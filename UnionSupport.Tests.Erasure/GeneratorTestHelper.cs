using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using UnionSupport.Generator.Erasure;

namespace UnionSupport.Tests.Erasure;

public static class GeneratorTestHelper
{
    private static readonly CSharpParseOptions ParseOptions = CSharpParseOptions.Default
        .WithLanguageVersion(LanguageVersion.Latest);

    private static readonly string[] CoreSources = new[]
    {
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
            public UnionImplAttribute(UnionImplementationStrategy strategy) { Strategy = strategy; }
        }
        """,
        """
        namespace System.Runtime.CompilerServices;
        public interface IUnion { object? Value { get; } }
        """
    };

    private static readonly MetadataReference[] Refs = new[]
    {
        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(System.Runtime.CompilerServices.Unsafe).Assembly.Location),
    };

    public static async Task VerifyAsync(string source, [System.Runtime.CompilerServices.CallerFilePath] string sourceFile = "")
    {
        var syntaxTrees = new List<SyntaxTree>();
        syntaxTrees.Add(CSharpSyntaxTree.ParseText(source, ParseOptions));

        foreach (var cs in CoreSources)
            syntaxTrees.Add(CSharpSyntaxTree.ParseText(cs, ParseOptions));

        var compilation = CSharpCompilation.Create(
            "TestAssembly",
            syntaxTrees,
            Refs,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        var compDiags = compilation.GetDiagnostics();
        var errors = compDiags.Where(d => d.Severity == DiagnosticSeverity.Error).ToList();

        GeneratorDriver driver = CSharpGeneratorDriver.Create(new ErasureUnionGenerator());

        driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generatorDiags);

        var generated = outputCompilation.SyntaxTrees
            .Skip(syntaxTrees.Count)
            .Select(t => t.ToString())
            .ToList();

        var sb = new System.Text.StringBuilder();

        if (errors.Count > 0)
        {
            sb.AppendLine("// COMPILATION ERRORS:");
            foreach (var e in errors)
                sb.AppendLine($"// {e}");
            sb.AppendLine();
        }

        if (generatorDiags.Length > 0)
        {
            sb.AppendLine("// GENERATOR DIAGNOSTICS:");
            foreach (var d in generatorDiags)
                sb.AppendLine($"// [{d.Severity}] {d}");
            sb.AppendLine();
        }

        if (generated.Count > 0)
            sb.AppendLine(string.Join("\n", generated));
        else
            sb.AppendLine("// No generated output");

        await Verifier.Verify(sb.ToString(), sourceFile: sourceFile);
    }

    public static Task VerifySource(string source, [System.Runtime.CompilerServices.CallerFilePath] string sourceFile = "")
    {
        return VerifyAsync(source, sourceFile);
    }
}
