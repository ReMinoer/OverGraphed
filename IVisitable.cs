namespace Diese.Graph
{
    public interface IVisitable<in TVisitor, TVertexBase, TEdgeBase> : IVisitable<TVisitor, TVertexBase, TVertexBase, TEdgeBase>
        where TVisitor : IVisitor<TVertexBase, TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
    }

    public interface IVisitable<in TVisitor, TVertex, TVertexBase, TEdgeBase>
        where TVisitor : IVisitor<TVertex, TVertexBase, TEdgeBase>
        where TVertex : TVertexBase
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        void Accept(TVisitor visitor);
    }
}