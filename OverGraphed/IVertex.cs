using System.Collections.Generic;

namespace OverGraphed
{
    public interface IVertex
    {
        IReadOnlyCollection<IEdge> Edges { get; }
        IReadOnlyCollection<IEdge> Predecessors { get; }
        IReadOnlyCollection<IEdge> Successors { get; }
        event Event<IEdge> EdgeAdded;
        event Event<IEdge> EdgeRemoved;
        event Event<IEdge> PredecessorAdded;
        event Event<IEdge> PredecessorRemoved;
        event Event<IEdge> SuccessorAdded;
        event Event<IEdge> SuccessorRemoved;
        event Event EdgesCleared;
    }

    public interface IVertex<out TVertexBase, out TEdgeBase> : IVertex
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        new IReadOnlyCollection<TEdgeBase> Edges { get; }
        new IReadOnlyCollection<TEdgeBase> Predecessors { get; }
        new IReadOnlyCollection<TEdgeBase> Successors { get; }
        new event Event<TEdgeBase> EdgeAdded;
        new event Event<TEdgeBase> EdgeRemoved;
        new event Event<TEdgeBase> PredecessorAdded;
        new event Event<TEdgeBase> PredecessorRemoved;
        new event Event<TEdgeBase> SuccessorAdded;
        new event Event<TEdgeBase> SuccessorRemoved;
    }

    public interface ILinkableVertex : IVertex
    {
        // TODO: Return boolean
        void UnlinkEdges();
    }

    // TODO: Check token
    public interface ILinkableVertex<TVertexBase, TEdgeBase> : ILinkableVertex, IVertex<TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        bool CanRegisterEdge(TEdgeBase edge, TVertexBase start, TVertexBase end);
        bool CanUnregisterEdge(TEdgeBase edge);
        bool RegisterEdge(TEdgeBase edge);
        bool UnregisterEdge(TEdgeBase edge);
    }
}