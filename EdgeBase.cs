namespace Diese.Graph
{
    public class EdgeBase<TEdge, TVertex> : IEdge<TEdge, TVertex>
        where TEdge : EdgeBase<TEdge, TVertex>
        where TVertex : VertexBase<TVertex, TEdge>
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
            var otherEdge = obj as EdgeBase<TEdge, TVertex>;
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