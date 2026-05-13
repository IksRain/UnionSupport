using UnionSupport;

// Product: Union<T1>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1>(T1 t1);

// Product: Union<T1, T2>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2>(T1 t1, T2 t2);

// Product: Union<T1, T2, T3>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3>(T1 t1, T2 t2, T3 t3);

// Product: Union<T1, T2, T3, T4>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);

// Product: Union<T1, T2, T3, T4, T5>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);

// Product: Union<T1, T2, T3, T4, T5, T6>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);

// Product: Union<T1, T2, T3, T4, T5, T6, T7>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);

// Product: Union<T1, T2, T3, T4, T5, T6, T7, T8>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);

// Product: Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9);

// Product: Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10);

// Product: Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11);

// Product: Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12);

// Product: Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13);

// Product: Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14);

// Product: Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15);

// Product: Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16);

// Product: Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
[UnionImpl(UnionImplementationStrategy.Product)]
partial struct Union<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17);

// Unmanaged: CUnion<T1>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1>(T1 t1) where T1 : unmanaged;

// Unmanaged: CUnion<T1, T2>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2>(T1 t1, T2 t2) where T1 : unmanaged where T2 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3>(T1 t1, T2 t2, T3 t3) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6, T7>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6, T7, T8>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged where T9 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged where T9 : unmanaged where T10 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged where T9 : unmanaged where T10 : unmanaged where T11 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged where T9 : unmanaged where T10 : unmanaged where T11 : unmanaged where T12 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged where T9 : unmanaged where T10 : unmanaged where T11 : unmanaged where T12 : unmanaged where T13 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged where T9 : unmanaged where T10 : unmanaged where T11 : unmanaged where T12 : unmanaged where T13 : unmanaged where T14 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged where T9 : unmanaged where T10 : unmanaged where T11 : unmanaged where T12 : unmanaged where T13 : unmanaged where T14 : unmanaged where T15 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged where T9 : unmanaged where T10 : unmanaged where T11 : unmanaged where T12 : unmanaged where T13 : unmanaged where T14 : unmanaged where T15 : unmanaged where T16 : unmanaged;

// Unmanaged: CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
[UnionImpl(UnionImplementationStrategy.Unmanaged)]
partial struct CUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17) where T1 : unmanaged where T2 : unmanaged where T3 : unmanaged where T4 : unmanaged where T5 : unmanaged where T6 : unmanaged where T7 : unmanaged where T8 : unmanaged where T9 : unmanaged where T10 : unmanaged where T11 : unmanaged where T12 : unmanaged where T13 : unmanaged where T14 : unmanaged where T15 : unmanaged where T16 : unmanaged where T17 : unmanaged;

// ObjectErasure: BoxedUnion<T1>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1>(T1 t1);

// ObjectErasure: BoxedUnion<T1, T2>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2>(T1 t1, T2 t2);

// ObjectErasure: BoxedUnion<T1, T2, T3>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3>(T1 t1, T2 t2, T3 t3);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4>(T1 t1, T2 t2, T3 t3, T4 t4);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6, T7>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16);

// ObjectErasure: BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>
[UnionImpl(UnionImplementationStrategy.ObjectErasure)]
partial struct BoxedUnion<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, T17>(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10, T11 t11, T12 t12, T13 t13, T14 t14, T15 t15, T16 t16, T17 t17);

