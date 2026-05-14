using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using UnionSupport.Generator.Erasure;

namespace UnionSupport.Tests.Erasure;

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
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """);
        var def = Activator.CreateInstance(t);
        Assert.Equal(false, t.GetProperty("HasValue")!.GetValue(def));
    }

    [Fact]
    public void HasValue_AfterConstructor_ReturnsTrue()
    {
        var t = CompileAndGetType("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """);
        var ctor = t.GetConstructor([typeof(int)])!;
        var union = ctor.Invoke([42]);
        Assert.Equal(true, t.GetProperty("HasValue")!.GetValue(union));
    }

    [Fact]
    public void TryGetValue_IntValue_ReturnsTrue()
    {
        var t = CompileAndGetType("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """);
        var ctor = t.GetConstructor([typeof(int)])!;
        var union = ctor.Invoke([42]);

        var args = new object[] { 0 };
        var found = t.GetMethod("TryGetValue", [typeof(int).MakeByRefType()])!.Invoke(union, args);
        Assert.Equal(true, found);
        Assert.Equal(42, args[0]);
    }

    [Fact]
    public void TryGetValue_StringValue_ReturnsTrue()
    {
        var t = CompileAndGetType("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """);
        var ctor = t.GetConstructor([typeof(string)])!;
        var union = ctor.Invoke(["hello"]);

        var args = new object[] { "" };
        var found = t.GetMethod("TryGetValue", [typeof(string).MakeByRefType()])!.Invoke(union, args);
        Assert.Equal(true, found);
        Assert.Equal("hello", args[0]);
    }

    [Fact]
    public void TryGetValue_WrongType_ReturnsFalse()
    {
        var t = CompileAndGetType("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """);
        var ctor = t.GetConstructor([typeof(int)])!;
        var union = ctor.Invoke([42]);

        var args = new object[] { "" };
        var found = t.GetMethod("TryGetValue", [typeof(string).MakeByRefType()])!.Invoke(union, args);
        Assert.Equal(false, found);
    }

    [Fact]
    public void IUnion_Value_ReturnsStoredObject()
    {
        var t = CompileAndGetType("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """);
        var ctor = t.GetConstructor([typeof(string)])!;
        var union = ctor.Invoke(["world"]);

        var iface = t.GetInterface("System.Runtime.CompilerServices.IUnion")!;
        var valueProp = iface.GetProperty("Value")!;
        Assert.Equal("world", valueProp.GetValue(union));
    }

    [Fact]
    public void IUnion_Value_Default_ReturnsNull()
    {
        var t = CompileAndGetType("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """);
        var def = Activator.CreateInstance(t);
        var iface = t.GetInterface("System.Runtime.CompilerServices.IUnion")!;
        var valueProp = iface.GetProperty("Value")!;
        Assert.Null(valueProp.GetValue(def));
    }

    [Fact]
    public void GeneratedCode_HasUnionAttribute()
    {
        var source = CompileAndGetGeneratedSource("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """);
        Assert.Contains("[global::System.Runtime.CompilerServices.Union]", source);
    }

    [Fact]
    public void GeneratedCode_ImplementsIUnion()
    {
        var source = CompileAndGetGeneratedSource("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """);
        Assert.Contains(": global::System.Runtime.CompilerServices.IUnion", source);
    }

    [Fact]
    public void GeneratedCode_UsesObjectField()
    {
        var source = CompileAndGetGeneratedSource("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """);
        Assert.Contains("object? __value", source);
        Assert.DoesNotContain("__private_flag", source);
    }

    [Fact]
    public void GeneratedCode_DoesNotHaveImplicitOperators()
    {
        var source = CompileAndGetGeneratedSource("""
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """);
        Assert.DoesNotContain("implicit operator", source);
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

        var driver = CSharpGeneratorDriver.Create(new ErasureUnionGenerator());
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

        var driver = CSharpGeneratorDriver.Create(new ErasureUnionGenerator());
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
