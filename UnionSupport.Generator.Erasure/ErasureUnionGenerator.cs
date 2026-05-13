using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UnionSupport.Generator.Shared;

namespace UnionSupport.Generator.Erasure;

[Generator]
public sealed class ErasureUnionGenerator : IIncrementalGenerator
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
                if (attr.ConstructorArguments.Length < 1)
                    return null;

                var strategyVal = GetEnumInt(attr.ConstructorArguments[0]);
                if (strategyVal != 2) // ObjectErasure
                    return null;

                return SourceGenHelpers.BuildTypeInfo(typeSymbol, "ObjectErasure", typeDecl, ctx.SemanticModel, ct);
            });

        context.RegisterSourceOutput(unionTypes.Where(t => t != null), (spc, info) =>
        {
            if (info == null) return;
            var src = UnionCodeGenerator.Generate(info.Value);
            spc.AddSource($"{info.Value.TypeName}.Erasure.g.cs", src);
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
