namespace OverGraphed
{
    public interface IEdge
    {
        IVertex Start { get; }
        IVertex End { get; }
    }

    public interface IEdge<out TVertexBase, out TEdgeBase> : IEdge
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        new TVertexBase Start { get; }
        new TVertexBase End { get; }
    }

    public interface IEdge<out TStartBase, out TEndBase, out TVertexBase, out TEdgeBase> : IEdge<TVertexBase, TEdgeBase>
        where TStartBase : TVertexBase
        where TEndBase : TVertexBase
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        new TStartBase Start { get; }
        new TEndBase End { get; }
    }

    public interface ILinkableEdge : IEdge
    {
        // TODO: Return boolean
        void Unlink();
    }

    public interface ILinkableEdge<out TVertexBase, out TEdgeBase> : ILinkableEdge, IEdge<TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
    }

    public interface ILinkableEdge<TStartBase, TEndBase, out TVertexBase, out TEdgeBase> : ILinkableEdge<TVertexBase, TEdgeBase>, IEdge<TStartBase, TEndBase, TVertexBase, TEdgeBase>
        where TStartBase : TVertexBase
        where TEndBase : TVertexBase
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        bool Link(TStartBase start, TEndBase end);
        bool ChangeStart(TStartBase start);
        bool ChangeEnd(TEndBase end);
    }
}