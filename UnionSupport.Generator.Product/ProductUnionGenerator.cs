using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnionSupport.Generator.Shared;

namespace UnionSupport.Generator.Product;

[Generator]
public sealed class ProductUnionGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var unionTypes = context.SyntaxProvider.ForAttributeWithMetadataName(
            SourceGenHelpers.AttributeFullName,
            predicate: (node, _) => node is StructDeclarationSyntax or ClassDeclarationSyntax,
            transform: (ctx, ct) =>
            {
                ct.ThrowIfCancellationRequested();
                if (ctx.TargetSymbol is not INamedTypeSymbol typeSymbol)
                    return null;
                if (ctx.TargetNode is not TypeDeclarationSyntax typeDecl)
                    return null;

                var attr = ctx.Attributes[0];
                // Default to Product when no argument specified
                var strategyVal = attr.ConstructorArguments.Length > 0
                    ? GetEnumInt(attr.ConstructorArguments[0])
                    : 0;
                if (strategyVal != 0) // Product
                    return null;

                return SourceGenHelpers.BuildTypeInfo(typeSymbol, "Product", typeDecl, ctx.SemanticModel, ct);
            });

        context.RegisterSourceOutput(unionTypes.Where(t => t != null), (spc, info) =>
        {
            if (info == null) return;
            var src = UnionCodeGenerator.Generate(info.Value);
            var suffix = info.Value.IsGeneric ? $"_{info.Value.GenericParameters.Count}" : "";
            spc.AddSource($"{info.Value.TypeName}{suffix}.Product.g.cs", src);
        });
    }

    private static int? GetEnumInt(TypedConstant constant)
    {
        if (constant.Value is int i) return i;
        if (constant.Value is byte b) return b;
        if (constant.Value is short s) return s;
        return null;
    }
}
