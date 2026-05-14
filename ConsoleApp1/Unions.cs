// Product strategy
[UnionSupport.UnionImpl]
partial struct MyUnion(int a, float b);

// Unmanaged strategy
[UnionSupport.UnionImpl(UnionSupport.UnionImplementationStrategy.Unmanaged)]
partial struct IntOrFloat(int a, float b);

// ObjectErasure strategy
[UnionSupport.UnionImpl(UnionSupport.UnionImplementationStrategy.ObjectErasure)]
partial struct AnyValue(int a, string b);

// ref struct Product
[UnionSupport.UnionImpl]
ref partial struct RefUnion(int a, float b);

// 0-length
[UnionSupport.UnionImpl]
partial struct Empty;
