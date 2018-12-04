namespace OverGraphed.Test.Utils
{
    public class TestEdgeReimplemented : ITestEdge
    {
        private readonly ILinkableEdge<ITestVertex, ITestVertex, ITestVertex, ITestEdge> _implementation;

        public TestEdgeReimplemented(bool refuseAllLink = false)
        {
            _implementation = new TestEdgeInherited(this, refuseAllLink);
        }

        public ITestVertex Start => _implementation.Start;
        public ITestVertex End => _implementation.End;
        IVertex IEdge.Start => _implementation.Start;
        IVertex IEdge.End => _implementation.End;
        
        public bool Link(ITestVertex start, ITestVertex end) => _implementation.Link(start, end);
        public bool ChangeStart(ITestVertex start) => _implementation.ChangeStart(start);
        public bool ChangeEnd(ITestVertex end) => _implementation.ChangeEnd(end);
        public void Unlink() => _implementation.Unlink();
    }
}