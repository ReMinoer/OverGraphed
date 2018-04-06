namespace OverGraphed
{
    public interface IVisitable<in TVisitor, TVertex, TVertexBase, TEdgeBase>
        where TVisitor : IVisitor<TVertex, TVertexBase, TEdgeBase>
        where TVertex : TVertexBase
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        void Accept(TVisitor visitor);
    }
}