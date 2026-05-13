namespace UnionSupport.Tests.Product;

public class ProductGeneratorTests
{
    [Fact]
    public Task SimpleUnion_IntFloat()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Product)]
            partial struct MyUnion(int a, float b);
            """;
        return GeneratorTestHelper.VerifySource(source);
    }

    [Fact]
    public Task SingleType()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Product)]
            partial struct SingleUnion(int value);
            """;
        return GeneratorTestHelper.VerifySource(source);
    }

    [Fact]
    public Task ZeroMembers()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Product)]
            partial struct EmptyUnion;
            """;
        return GeneratorTestHelper.VerifySource(source);
    }

    [Fact]
    public Task ThreeTypes()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Product)]
            partial struct TripleUnion(int a, string b, double c);
            """;
        return GeneratorTestHelper.VerifySource(source);
    }

    [Fact]
    public Task RefStruct_Product()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Product)]
            ref partial struct RefUnion(int a, float b);
            """;
        return GeneratorTestHelper.VerifySource(source);
    }

    [Fact]
    public Task DefaultStrategy_Product()
    {
        var source = """
            using UnionSupport;
            [UnionImpl]
            partial struct DefaultUnion(int a, float b);
            """;
        return GeneratorTestHelper.VerifySource(source);
    }
}
