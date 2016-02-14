using System.Collections.Generic;

namespace Diese.Graph
{
    public interface IVertex<out TVertexBase, out TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        IEnumerable<TEdgeBase> Edges { get; }
        IEnumerable<TVertexBase> Predecessors { get; }
        IEnumerable<TVertexBase> Successors { get; }
    }
}