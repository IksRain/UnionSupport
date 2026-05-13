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

    public static readonly DiagnosticDescriptor RefStructStrategyRule = new(
        "UNION002",
        "ref struct union must use Product strategy",
        "ref struct union '{0}' must use Product strategy. Unmanaged and ObjectErasure are not supported for ref struct types",
        "UnionSupport",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DuplicateRule, RefStructStrategyRule);

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

        var unionAttr = typeDecl.AttributeLists
            .SelectMany(al => al.Attributes)
            .FirstOrDefault(a => a.Name.ToString().Contains(AttributeName));

        if (unionAttr == null) return;

        var isRefStruct = typeDecl is StructDeclarationSyntax sds
            && sds.Modifiers.Any(SyntaxKind.RefKeyword);

        // Extract strategy value
        int strategyVal = -1;
        if (unionAttr.ArgumentList?.Arguments.Count > 0)
        {
            var arg = unionAttr.ArgumentList.Arguments[0];
            var typeInfo = context.SemanticModel.GetTypeInfo(arg.Expression, context.CancellationToken);
            if (typeInfo.Type is INamedTypeSymbol)
            {
                // Enum member access like UnionImplementationStrategy.Product
                var expr = arg.Expression.ToString();
                if (expr.EndsWith(".Product")) strategyVal = 0;
                else if (expr.EndsWith(".Unmanaged")) strategyVal = 1;
                else if (expr.EndsWith(".ObjectErasure")) strategyVal = 2;
            }
        }

        // UNION002: ref struct must use Product
        if (isRefStruct && strategyVal != 0 && strategyVal != -1)
        {
            context.ReportDiagnostic(Diagnostic.Create(RefStructStrategyRule,
                unionAttr.GetLocation(),
                typeDecl.Identifier.Text));
            return; // don't run duplicate check on invalid ref struct unions
        }

        // UNION001: duplicate type check
        if (typeDecl.ParameterList == null) return;

        var concreteSeen = new Dictionary<string, ParameterSyntax>();
        var typeParamSeen = new Dictionary<string, ParameterSyntax>();

        foreach (var param in typeDecl.ParameterList.Parameters)
        {
            if (param.Type == null) continue;

            var ti = context.SemanticModel.GetTypeInfo(param.Type, context.CancellationToken);

            if (ti.Type is ITypeParameterSymbol tp)
            {
                if (typeParamSeen.TryGetValue(tp.Name, out _))
                {
                    context.ReportDiagnostic(Diagnostic.Create(DuplicateRule,
                        param.GetLocation(), tp.Name, typeDecl.Identifier.Text));
                }
                else
                {
                    typeParamSeen[tp.Name] = param;
                }
            }
            else if (ti.Type is INamedTypeSymbol namedType)
            {
                var key = namedType.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
                if (concreteSeen.TryGetValue(key, out _))
                {
                    context.ReportDiagnostic(Diagnostic.Create(DuplicateRule,
                        param.GetLocation(), param.Type.ToString(), typeDecl.Identifier.Text));
                }
                else
                {
                    concreteSeen[key] = param;
                }
            }
        }
    }
}
