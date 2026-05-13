using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UnionSupport.Generator.Shared;

internal static class SourceGenHelpers
{
    public const string AttributeFullName = "UnionSupport.UnionImplAttribute";
    public const string ToolName = "UnionSupport";
    public const string ToolVersion = "1.0.0";

    private static readonly SymbolDisplayFormat FullTypeFormat = new SymbolDisplayFormat(
        typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameAndContainingTypesAndNamespaces,
        genericsOptions: SymbolDisplayGenericsOptions.IncludeTypeParameters,
        miscellaneousOptions: SymbolDisplayMiscellaneousOptions.ExpandNullable);

    public static string EncodeFieldName(ITypeSymbol type)
    {
        if (type is ITypeParameterSymbol tp)
            return $"__{tp.Name}";

        var fqn = type.ToDisplayString(FullTypeFormat);
        fqn = fqn.Replace(".", "_");
        fqn = fqn.Replace("+", "_");
        fqn = fqn.Replace("<", "__");
        fqn = fqn.Replace(">", "__");
        fqn = fqn.Replace(", ", "___");
        return $"__{fqn}";
    }

    public static UnionTypeInfo? BuildTypeInfo(
        INamedTypeSymbol typeSymbol,
        string strategyStr,
        TypeDeclarationSyntax typeDecl,
        SemanticModel semanticModel,
        CancellationToken ct)
    {
        var members = new List<UnionMemberInfo>();
        int index = 0;

        if (typeDecl.ParameterList != null)
        {
            foreach (var param in typeDecl.ParameterList.Parameters)
            {
                if (param.Type == null) continue;

                var paramType = semanticModel.GetTypeInfo(param.Type, ct).Type;
                if (paramType == null) continue;

                var displayType = paramType.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);
                var isTp = paramType is ITypeParameterSymbol;
                var fieldName = EncodeFieldName(paramType);

                members.Add(new UnionMemberInfo(displayType, index, isTp, fieldName, paramType));
                index++;
            }
        }

        var genericParams = new List<string>();
        if (typeSymbol.IsGenericType)
        {
            foreach (var tp in typeSymbol.TypeParameters)
                genericParams.Add(tp.Name);
        }

        var nsDisplay = typeSymbol.ContainingNamespace?.ToDisplayString() ?? "";
        if (nsDisplay == "<global namespace>")
            nsDisplay = "";

        var isRefStruct = typeDecl.Modifiers.Any(SyntaxKind.RefKeyword);

        return new UnionTypeInfo(
            TypeName: typeSymbol.Name,
            Namespace: nsDisplay,
            IsValueType: typeSymbol.IsValueType,
            IsRefStruct: isRefStruct,
            IsGeneric: typeSymbol.IsGenericType,
            GenericParameters: genericParams,
            Members: members,
            StrategyName: strategyStr
        );
    }
}

internal readonly record struct UnionTypeInfo(
    string TypeName,
    string Namespace,
    bool IsValueType,
    bool IsRefStruct,
    bool IsGeneric,
    List<string> GenericParameters,
    List<UnionMemberInfo> Members,
    string StrategyName
);

internal readonly record struct UnionMemberInfo(
    string DisplayType,
    int Index,
    bool IsTypeParameter,
    string FieldName,
    ITypeSymbol TypeSymbol
);
