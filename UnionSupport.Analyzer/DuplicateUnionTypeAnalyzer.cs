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

    public static readonly DiagnosticDescriptor RefStructRule = new(
        "UNION002",
        "Ref struct constraint violation in union",
        "{0}",
        "UnionSupport",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
        = ImmutableArray.Create(DuplicateRule, RefStructRule);

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

        int strategyVal = -1;
        if (unionAttr.ArgumentList?.Arguments.Count > 0)
        {
            var expr = unionAttr.ArgumentList.Arguments[0].Expression.ToString();
            if (expr.EndsWith(".Product")) strategyVal = 0;
            else if (expr.EndsWith(".Unmanaged")) strategyVal = 1;
            else if (expr.EndsWith(".ObjectErasure")) strategyVal = 2;
        }

        // UNION002: ref struct union must use Product
        if (isRefStruct && strategyVal != 0 && strategyVal != -1)
        {
            context.ReportDiagnostic(Diagnostic.Create(RefStructRule,
                unionAttr.GetLocation(),
                $"Ref struct union '{typeDecl.Identifier.Text}' must use Product strategy"));
            return;
        }

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
                // UNION002: ref struct type on non-ref struct union
                if (namedType.IsRefLikeType && !isRefStruct)
                {
                    context.ReportDiagnostic(Diagnostic.Create(RefStructRule,
                        param.GetLocation(),
                        $"Type '{param.Type}' is a ref struct and cannot be a member of non-ref struct union '{typeDecl.Identifier.Text}'"));
                    continue;
                }

                // UNION001: duplicate check
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
