using System.Linq;

namespace OverGraphed.Base
{
    public abstract class LinkableVertexBase<TVertexBase, TEdgeBase> : VertexBase<TVertexBase, TEdgeBase>, ILinkableVertex<TVertexBase, TEdgeBase>
        where TVertexBase : class, ILinkableVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, ILinkableEdge<TVertexBase, TEdgeBase>
    {
        public override event Event EdgesCleared;

        protected LinkableVertexBase()
        {
        }

        protected LinkableVertexBase(TVertexBase owner)
            : base(owner)
        {
        }

        public void UnlinkEdges()
        {
            foreach (TEdgeBase edge in PredecessorsBasesExplicit.ToArray())
                edge.Unlink();
            foreach (TEdgeBase edge in SuccessorsBasesExplicit.ToArray())
                edge.Unlink();

            EdgesCleared?.Invoke(this);
        }

        protected abstract bool CanRegisterEdge(TEdgeBase edge, TVertexBase start, TVertexBase end);
        protected abstract bool CanUnregisterEdge(TEdgeBase edge);
        protected abstract bool RegisterEdge(TEdgeBase edge);
        protected abstract bool UnregisterEdge(TEdgeBase edge);

        bool ILinkableVertex<TVertexBase, TEdgeBase>.CanRegisterEdge(TEdgeBase edge, TVertexBase start, TVertexBase end) => CanRegisterEdge(edge, start, end);
        bool ILinkableVertex<TVertexBase, TEdgeBase>.CanUnregisterEdge(TEdgeBase edge) => CanUnregisterEdge(edge);
        bool ILinkableVertex<TVertexBase, TEdgeBase>.RegisterEdge(TEdgeBase edge) => RegisterEdge(edge);
        bool ILinkableVertex<TVertexBase, TEdgeBase>.UnregisterEdge(TEdgeBase edge) => UnregisterEdge(edge);
    }
}