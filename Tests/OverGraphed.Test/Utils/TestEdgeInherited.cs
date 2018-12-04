namespace OverGraphed.Test.Utils
{
    public class TestEdgeInherited : Edge<ITestVertex, ITestEdge>, ITestEdge
    {
        private readonly bool _refuseAllLink;

        public TestEdgeInherited(bool refuseAllLink = false)
        {
            _refuseAllLink = refuseAllLink;
        }

        public TestEdgeInherited(ITestEdge owner, bool refuseAllLink = false)
            : base(owner)
        {
            _refuseAllLink = refuseAllLink;
        }

        protected override bool CanLinkVertices(ITestVertex newStart, ITestVertex newEnd) => base.CanLinkVertices(newStart, newEnd) && !_refuseAllLink;
    }
}