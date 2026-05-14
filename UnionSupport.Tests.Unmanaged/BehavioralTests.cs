using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using UnionSupport.Generator.Unmanaged;

namespace UnionSupport.Tests.Unmanaged;

public sealed class BehavioralTests
{
    private static readonly CSharpParseOptions ParseOptions = CSharpParseOptions.Default
        .WithLanguageVersion(LanguageVersion.Latest);

    private static readonly string[] CoreSources =
    [
        """
        namespace UnionSupport;
        public enum UnionImplementationStrategy { Product = 0, Unmanaged = 1, ObjectErasure = 2 }
        """,
        """
        namespace UnionSupport;
        [System.AttributeUsage(System.AttributeTargets.Struct | System.AttributeTargets.Class, AllowMultiple = false)]
        public sealed class UnionImplAttribute : System.Attribute
        {
            public UnionImplementationStrategy Strategy { get; }
            public UnionImplAttribute(UnionImplementationStrategy strategy = UnionImplementationStrategy.Product) { Strategy = strategy; }
        }
        """
    ];

    private static readonly MetadataReference[] Refs;

    static BehavioralTests()
    {
        var list = new List<MetadataReference>
        {
            MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
            MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location),
        };

        var runtimeDir = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
        var systemRuntime = Path.Combine(runtimeDir, "System.Runtime.dll");
        if (File.Exists(systemRuntime))
            list.Add(MetadataReference.CreateFromFile(systemRuntime));

        Refs = list.ToArray();
    }

    [Fact]
    public void HasValue_DefaultStruct_ReturnsFalse()
    {
        var t = CompileAndGetType("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """);
        var def = Activator.CreateInstance(t);
        Assert.Equal(false, t.GetProperty("HasValue")!.GetValue(def));
    }

    [Fact]
    public void HasValue_AfterConstructor_ReturnsTrue()
    {
        var t = CompileAndGetType("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """);
        var ctor = t.GetConstructor([typeof(int)])!;
        var union = ctor.Invoke([42]);
        Assert.Equal(true, t.GetProperty("HasValue")!.GetValue(union));
    }

    [Fact]
    public void TryGetValue_MatchesType_ReturnsTrueAndValue()
    {
        var t = CompileAndGetType("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """);
        var ctor = t.GetConstructor([typeof(int)])!;
        var union = ctor.Invoke([42]);

        var args = new object[] { 0 };
        var found = t.GetMethod("TryGetValue", [typeof(int).MakeByRefType()])!.Invoke(union, args);
        Assert.Equal(true, found);
        Assert.Equal(42, args[0]);
    }

    [Fact]
    public void TryGetValue_WrongType_ReturnsFalse()
    {
        var t = CompileAndGetType("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """);
        var ctor = t.GetConstructor([typeof(int)])!;
        var union = ctor.Invoke([42]);

        var args = new object[] { 0f };
        var found = t.GetMethod("TryGetValue", [typeof(float).MakeByRefType()])!.Invoke(union, args);
        Assert.Equal(false, found);
    }

    [Fact]
    public void IUnion_Value_ReturnsStoredObject()
    {
        var t = CompileAndGetType("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """);
        var ctor = t.GetConstructor([typeof(int)])!;
        var union = ctor.Invoke([42]);

        var iface = t.GetInterface("System.Runtime.CompilerServices.IUnion")!;
        var valueProp = iface.GetProperty("Value")!;
        Assert.Equal(42, valueProp.GetValue(union));
    }

    [Fact]
    public void GeneratedCode_HasExplicitLayout()
    {
        var source = CompileAndGetGeneratedSource("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """);
        Assert.Contains("StructLayout", source);
        Assert.Contains("LayoutKind.Explicit", source);
        Assert.Contains("FieldOffset", source);
    }

    [Fact]
    public void GeneratedCode_HasUnionAttribute()
    {
        var source = CompileAndGetGeneratedSource("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """);
        Assert.Contains("[global::System.Runtime.CompilerServices.Union]", source);
    }

    [Fact]
    public void GeneratedCode_HasHasValue()
    {
        var source = CompileAndGetGeneratedSource("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """);
        Assert.Contains("public bool HasValue", source);
    }

    [Fact]
    public void GeneratedCode_HasTryGetValue()
    {
        var source = CompileAndGetGeneratedSource("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """);
        Assert.Contains("public bool TryGetValue(out int value)", source);
        Assert.Contains("public bool TryGetValue(out float value)", source);
    }

    [Fact]
    public void GeneratedCode_DoesNotHaveImplicitOperators()
    {
        var source = CompileAndGetGeneratedSource("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """);
        Assert.DoesNotContain("implicit operator", source);
    }

    [Fact]
    public void GeneratedCode_ImplementsIUnion()
    {
        var source = CompileAndGetGeneratedSource("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """);
        Assert.Contains(": global::System.Runtime.CompilerServices.IUnion", source);
    }

    private Type CompileAndGetType(string source)
    {
        var asm = CompileToAssembly(source);
        return asm.GetType(ExtractTypeName(source))!;
    }

    private static string CompileAndGetGeneratedSource(string source)
    {
        var trees = new List<SyntaxTree> { CSharpSyntaxTree.ParseText(source, ParseOptions) };
        trees.AddRange(CoreSources.Select(cs => CSharpSyntaxTree.ParseText(cs, ParseOptions)));

        var compilation = CSharpCompilation.Create(
            "TestAssembly", trees, Refs,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(new UnmanagedUnionGenerator());
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out _);

        var generated = output.SyntaxTrees.Skip(trees.Count).Select(t => t.ToString()).ToList();
        return string.Join("\n", generated);
    }

    private static string ExtractTypeName(string source)
    {
        var match = Regex.Match(source, @"partial\s+struct\s+(\w+)");
        return match.Success ? match.Groups[1].Value : "Unknown";
    }

    private Assembly CompileToAssembly(string source)
    {
        var trees = new List<SyntaxTree> { CSharpSyntaxTree.ParseText(source, ParseOptions) };
        trees.AddRange(CoreSources.Select(cs => CSharpSyntaxTree.ParseText(cs, ParseOptions)));

        var compilation = CSharpCompilation.Create(
            "TestAssembly", trees, Refs,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        var driver = CSharpGeneratorDriver.Create(new UnmanagedUnionGenerator());
        driver.RunGeneratorsAndUpdateCompilation(compilation, out var output, out _);

        using var ms = new MemoryStream();
        var result = output.Emit(ms);
        if (!result.Success)
        {
            var errors = string.Join("\n", result.Diagnostics.Where(d => d.Severity == DiagnosticSeverity.Error));
            throw new InvalidOperationException($"Compilation errors:\n{errors}");
        }

        return Assembly.Load(ms.ToArray());
    }
}
