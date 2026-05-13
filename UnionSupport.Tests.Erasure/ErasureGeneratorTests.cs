namespace UnionSupport.Tests.Erasure;

public class ErasureGeneratorTests
{
    [Fact]
    public Task SimpleUnion_IntString()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct MyUnion(int a, string b);
            """;
        return GeneratorTestHelper.VerifySource(source);
    }

    [Fact]
    public Task SingleType()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct SingleUnion(double value);
            """;
        return GeneratorTestHelper.VerifySource(source);
    }

    [Fact]
    public Task ThreeTypes_ValueAndRef()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.ObjectErasure)]
            partial struct TripleUnion(int a, string b, bool c);
            """;
        return GeneratorTestHelper.VerifySource(source);
    }
}
