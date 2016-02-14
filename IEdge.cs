namespace Diese.Graph
{
    public interface IEdge<out TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        TVertexBase Start { get; }
        TVertexBase End { get; }
    }

    public interface IEdge<out TStart, out TEnd, out TVertexBase, TEdgeBase> : IEdge<TVertexBase, TEdgeBase>
        where TStart : TVertexBase
        where TEnd : TVertexBase
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        new TStart Start { get; }
        new TEnd End { get; }
    }
}