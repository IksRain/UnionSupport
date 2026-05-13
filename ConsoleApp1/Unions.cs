// Product strategy
[UnionSupport.UnionImpl(UnionSupport.UnionImplementationStrategy.Product)]
partial struct MyUnion(int? a, float b);

// Unmanaged strategy
[UnionSupport.UnionImpl(UnionSupport.UnionImplementationStrategy.Unmanaged)]
partial struct IntOrFloat(int a, float b);

// ObjectErasure strategy
[UnionSupport.UnionImpl(UnionSupport.UnionImplementationStrategy.ObjectErasure)]
partial struct AnyValue(int a, string b);

// 0-length
[UnionSupport.UnionImpl(UnionSupport.UnionImplementationStrategy.Product)]
partial struct Empty;
