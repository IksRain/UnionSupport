namespace UnionSupport.Tests.Unmanaged;

public class UnmanagedGeneratorTests
{
    [Fact]
    public Task SimpleUnion_IntFloat()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct MyUnion(int a, float b);
            """;
        return GeneratorTestHelper.VerifySource(source);
    }

    [Fact]
    public Task SingleType()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct SingleUnion(int value);
            """;
        return GeneratorTestHelper.VerifySource(source);
    }

    [Fact]
    public Task ThreeTypes()
    {
        var source = """
            using UnionSupport;
            [UnionImpl(UnionImplementationStrategy.Unmanaged)]
            partial struct TripleUnion(int a, long b, double c);
            """;
        return GeneratorTestHelper.VerifySource(source);
    }
}
