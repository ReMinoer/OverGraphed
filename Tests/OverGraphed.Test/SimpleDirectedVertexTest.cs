using System;
using OverGraphed.Test.Base;
using OverGraphed.Test.Utils;

namespace OverGraphed.Test
{
    public class SimpleDirectedVertexTest : SimpleDirectedVertexTestBase
    {
        protected override ITestVertex GetVertex(ImplementationType type, bool refuseAllRegistration = false)
        {
            switch (type)
            {
                case ImplementationType.Inherited:
                    return new TestVertexInherited(refuseAllRegistration);
                case ImplementationType.Reimplemented:
                    return new TestVertexReimplemented(refuseAllRegistration);
                default:
                    throw new NotSupportedException();
            }
        }

        protected override void InvalidVertexConstructor()
        {
            var _ = new TestVertexInvalid();
        }

        public class TestVertexInherited : SimpleDirectedVertex<ITestVertex, ITestEdge>, ITestVertex
        {
            private readonly bool _refuseAllRegistration;

            public TestVertexInherited(bool refuseAllRegistration)
            {
                _refuseAllRegistration = refuseAllRegistration;
            }

            public TestVertexInherited(ITestVertex owner, bool refuseAllRegistration)
                : base(owner)
            {
                _refuseAllRegistration = refuseAllRegistration;
            }

            protected override bool CanRegister(ITestEdge edge, ITestVertex start, ITestVertex end) => base.CanRegister(edge, start, end) && !_refuseAllRegistration;
            protected override bool CanUnregister(ITestEdge edge) => base.CanUnregister(edge) && !_refuseAllRegistration;
        }

        public class TestVertexReimplemented : TestVertexReimplementedBase
        {
            public TestVertexReimplemented(bool refuseAllRegistration)
            {
                Implementation = new TestVertexInherited(this, refuseAllRegistration);
            }
        }

        public class TestVertexInvalid : SimpleDirectedVertex<ITestVertex, ITestEdge>
        {
        }
    }
}
