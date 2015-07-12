namespace Diese.Graph
{
    public interface IEdge<TEdge, out TVertex>
        where TEdge : IEdge<TEdge, TVertex>
        where TVertex : IVertex<TVertex, TEdge>
    {
        TVertex Start { get; }
        TVertex End { get; }
    }
}