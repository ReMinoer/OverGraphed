using System.Collections.Generic;

namespace OverGraphed
{
    public interface IVertex<out TVertexBase, out TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        IEnumerable<TEdgeBase> Predecessors { get; }
        IEnumerable<TEdgeBase> Successors { get; }
    }
}