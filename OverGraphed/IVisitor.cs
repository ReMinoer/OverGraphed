namespace OverGraphed
{
    public interface IVisitor<TVertexBase, TEdgeBase> : IVisitor<TVertexBase, TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
    }

    public interface IVisitor<in TVertex, TVertexBase, TEdgeBase>
        where TVertex : TVertexBase
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        void Visit(TVertex vertex);
    }
}