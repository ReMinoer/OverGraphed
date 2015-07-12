namespace Diese.Graph
{
    public class EdgeBase<TGraph, TVertex, TEdge, TVisitor> : IEdge<TGraph, TVertex, TEdge, TVisitor>
        where TGraph : GraphBase<TGraph, TVertex, TEdge, TVisitor>
        where TVertex : VertexBase<TGraph, TVertex, TEdge, TVisitor>
        where TEdge : EdgeBase<TGraph, TVertex, TEdge, TVisitor>
        where TVisitor : VisitorBase<TGraph, TVertex, TEdge, TVisitor>
    {
        public TVertex Start { get; private set; }
        public TVertex End { get; private set; }

        public EdgeBase(TVertex start, TVertex end)
        {
            Start = start;
            End = end;
        }

        public override bool Equals(object obj)
        {
            var otherEdge = obj as EdgeBase<TGraph, TVertex, TEdge, TVisitor>;
            if (otherEdge == null)
                return false;

            return Start == otherEdge.Start && End == otherEdge.End;
        }

        public override int GetHashCode()
        {
            return Start.GetHashCode() ^ End.GetHashCode();
        }
    }
}