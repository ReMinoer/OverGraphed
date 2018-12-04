using System;

namespace OverGraphed
{
    public class Edge<TVertexBase> : Edge<TVertexBase, TVertexBase, TVertexBase, Edge<TVertexBase>>
        where TVertexBase : class, ILinkableVertex<TVertexBase, Edge<TVertexBase>>
    {
        public Edge()
        {
        }

        public Edge(Edge<TVertexBase> owner)
            : base(owner)
        {
        }
    }

    public abstract class Edge<TVertexBase, TEdgeBase> : Edge<TVertexBase, TVertexBase, TVertexBase, TEdgeBase>
        where TVertexBase : class, ILinkableVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, ILinkableEdge<TVertexBase, TEdgeBase>
    {
        protected Edge()
        {
        }

        protected Edge(TEdgeBase owner)
            : base(owner)
        {
        }
    }

    public abstract class Edge<TStartBase, TEndBase, TVertexBase, TEdgeBase> : ILinkableEdge<TStartBase, TEndBase, TVertexBase, TEdgeBase>
        where TStartBase : class, TVertexBase
        where TEndBase : class, TVertexBase
        where TVertexBase : class, ILinkableVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, ILinkableEdge<TVertexBase, TEdgeBase>
    {
        protected readonly TEdgeBase Owner;

        public TStartBase Start { get; private set; }
        public TEndBase End { get; private set; }

        TVertexBase IEdge<TVertexBase, TEdgeBase>.Start => Start;
        TVertexBase IEdge<TVertexBase, TEdgeBase>.End => End;
        IVertex IEdge.Start => Start;
        IVertex IEdge.End => End;

        protected Edge()
        {
            Owner = this as TEdgeBase;
            if (Owner == null)
                throw new InvalidOperationException();
        }

        protected Edge(TEdgeBase owner)
        {
            Owner = owner ?? throw new InvalidOperationException();
        }

        public bool Link(TStartBase start, TEndBase end)
        {
            if (start == null)
                throw new ArgumentNullException(nameof(start));
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            if (!CanLink(start, end))
                return false;

            LinkInternal(start, end);
            return true;
        }

        public bool ChangeStart(TStartBase start)
        {
            if (Start == null || End == null)
                throw new InvalidOperationException();
            if (start == null)
                throw new ArgumentNullException(nameof(start));

            if (!CanLink(start, End))
                return false;

            LinkInternal(start, End);
            return true;
        }

        public bool ChangeEnd(TEndBase end)
        {
            if (Start == null || End == null)
                throw new InvalidOperationException();
            if (end == null)
                throw new ArgumentNullException(nameof(end));

            if (!CanLink(Start, end))
                return false;

            LinkInternal(Start, end);
            return true;
        }

        public void Unlink()
        {
            if (Start == null || End == null)
                throw new InvalidOperationException();

            Start.UnregisterEdge(Owner);
            End.UnregisterEdge(Owner);

            Start = null;
            End = null;
        }

        private void LinkInternal<TStart, TEnd>(TStart start, TEnd end)
            where TStart : TStartBase where TEnd : TEndBase
        {
            Start?.UnregisterEdge(Owner);
            End?.UnregisterEdge(Owner);

            Start = start;
            End = end;

            Start?.RegisterEdge(Owner);
            End?.RegisterEdge(Owner);
        }

        private bool CanLink(TStartBase newStart, TEndBase newEnd)
        {
            return (Start == null || Start.CanUnregisterEdge(Owner))
                   && (newStart == null || newStart.CanRegisterEdge(Owner, newStart, newEnd))
                   && (End == null || End.CanUnregisterEdge(Owner))
                   && (newEnd == null || newEnd.CanRegisterEdge(Owner, newStart, newEnd))
                   && CanLinkVertices(newStart, newEnd);
        }

        protected virtual bool CanLinkVertices(TStartBase newStart, TEndBase newEnd) => true;
    }
}