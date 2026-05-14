using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;
using UnionSupport.Analyzer;

namespace UnionSupport.Tests.Analyzer;

public static class AnalyzerTestHelper
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
            public UnionImplAttribute(UnionImplementationStrategy strategy = UnionImplementationStrategy.Product) { Strategy = strategy; }
        }
        """
    };

    private static readonly MetadataReference[] Refs = new[]
    {
        MetadataReference.CreateFromFile(typeof(object).Assembly.Location),
        MetadataReference.CreateFromFile(typeof(System.Linq.Enumerable).Assembly.Location),
    };

    public static async Task VerifyDiagnosticsAsync(string source, [System.Runtime.CompilerServices.CallerFilePath] string sourceFile = "")
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

        var analyzer = new DuplicateUnionTypeAnalyzer();
        var compilationWithAnalyzers = compilation.WithAnalyzers(
            ImmutableArray.Create<DiagnosticAnalyzer>(analyzer));

        var diagnostics = await compilationWithAnalyzers.GetAnalyzerDiagnosticsAsync();

        var result = new System.Text.StringBuilder();
        if (diagnostics.Length == 0)
        {
            result.AppendLine("// No diagnostics");
        }
        else
        {
            foreach (var d in diagnostics.OrderBy(d => d.Id).ThenBy(d => d.Location.GetLineSpan().StartLinePosition.Line))
                result.AppendLine($"// [{d.Id}] {d.GetMessage()}");
        }

        await Verifier.Verify(result.ToString(), sourceFile: sourceFile);
    }

    public static Task Verify(string source, [System.Runtime.CompilerServices.CallerFilePath] string sourceFile = "")
        => VerifyDiagnosticsAsync(source, sourceFile);
}
