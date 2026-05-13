using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace UnionSupport.Analyzer;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public sealed class DuplicateUnionTypeAnalyzer : DiagnosticAnalyzer
{
    private const string AttributeName = "UnionImpl";

    public static readonly DiagnosticDescriptor DuplicateRule = new(
        "UNION001",
        "Duplicate type in union declaration",
        "Type '{0}' appears more than once in union '{1}'",
        "UnionSupport",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DuplicateRule);

    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(AnalyzeTypeDeclaration,
            SyntaxKind.StructDeclaration, SyntaxKind.ClassDeclaration);
    }

    private static void AnalyzeTypeDeclaration(SyntaxNodeAnalysisContext context)
    {
        var typeDecl = (TypeDeclarationSyntax)context.Node;

        var hasAttr = typeDecl.AttributeLists
            .SelectMany(al => al.Attributes)
            .Any(a => a.Name.ToString().Contains(AttributeName));

        if (!hasAttr) return;

        var symbol = context.SemanticModel.GetDeclaredSymbol(typeDecl, context.CancellationToken);
        if (symbol is not INamedTypeSymbol typeSymbol) return;
        if (typeSymbol.TypeKind != TypeKind.Struct && typeSymbol.TypeKind != TypeKind.Class) return;
        if (typeDecl.ParameterList == null) return;

        // Track concrete types (by FQN) and type parameters (by name) separately
        var concreteSeen = new Dictionary<string, ParameterSyntax>();
        var typeParamSeen = new Dictionary<string, ParameterSyntax>();

        foreach (var param in typeDecl.ParameterList.Parameters)
        {
            if (param.Type == null) continue;

            var typeInfo = context.SemanticModel.GetTypeInfo(param.Type, context.CancellationToken);

            if (typeInfo.Type is ITypeParameterSymbol tp)
            {
                if (typeParamSeen.TryGetValue(tp.Name, out var existing))
                {
                    context.ReportDiagnostic(Diagnostic.Create(DuplicateRule, param.GetLocation(),
                        tp.Name, typeSymbol.Name));
                }
                else
                {
                    typeParamSeen[tp.Name] = param;
                }
            }
            else if (typeInfo.Type is INamedTypeSymbol namedType)
            {
                var key = namedType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (concreteSeen.TryGetValue(key, out var existing))
                {
                    context.ReportDiagnostic(Diagnostic.Create(DuplicateRule, param.GetLocation(),
                        param.Type.ToString(), typeSymbol.Name));
                }
                else
                {
                    concreteSeen[key] = param;
                }
            }
        }
    }
}
