using System.Collections.Generic;

namespace OverGraphed
{
    // TODO: Graph-wrapped vertices
    // TODO: Graph as vertex
    public interface IGraph
    {
        IEnumerable<IVertex> Vertices { get; }
        IEnumerable<IEdge> Edges { get; }
        event Event<IVertex> VertexAdded;
        event Event<IVertex> VertexRemoved;
        event Event<IEdge> EdgeAdded;
        event Event<IEdge> EdgeRemoved;
        event Event Cleared;
    }

    public interface IGraph<out TVertexBase, out TEdgeBase> : IGraph
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        new IEnumerable<TVertexBase> Vertices { get; }
        new IEnumerable<TEdgeBase> Edges { get; }
        new event Event<TVertexBase> VertexAdded;
        new event Event<TVertexBase> VertexRemoved;
        new event Event<TEdgeBase> EdgeAdded;
        new event Event<TEdgeBase> EdgeRemoved;
    }
}