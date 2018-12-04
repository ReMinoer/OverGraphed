using System.Collections.Generic;
using OverGraphed.Utils;

namespace OverGraphed.Base
{
    public abstract class GraphBase<TVertexBase, TEdgeBase> : IGraph<TVertexBase, TEdgeBase>
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        protected readonly HashSet<TVertexBase> VerticesSet;
        protected readonly HashSet<TEdgeBase> EdgesSet;

        public ReadOnlyHashSet<TVertexBase> Vertices { get; }
        public ReadOnlyHashSet<TEdgeBase> Edges { get; }

        public abstract event Event<TVertexBase> VertexAdded;
        public abstract event Event<TVertexBase> VertexRemoved;
        public abstract event Event<TEdgeBase> EdgeAdded;
        public abstract event Event<TEdgeBase> EdgeRemoved;
        public abstract event Event Cleared;

        protected abstract event Event<IVertex> VertexAddedExplicit;
        protected abstract event Event<IVertex> VertexRemovedExplicit;
        protected abstract event Event<IEdge> EdgeAddedExplicit;
        protected abstract event Event<IEdge> EdgeRemovedExplicit;

        IEnumerable<IVertex> IGraph.Vertices => Vertices;
        IEnumerable<IEdge> IGraph.Edges => Edges;
        IEnumerable<TVertexBase> IGraph<TVertexBase, TEdgeBase>.Vertices => Vertices;
        IEnumerable<TEdgeBase> IGraph<TVertexBase, TEdgeBase>.Edges => Edges;

        event Event<IVertex> IGraph.VertexAdded
        {
            add => VertexAddedExplicit += value;
            remove => VertexAddedExplicit -= value;
        }
        event Event<IVertex> IGraph.VertexRemoved
        {
            add => VertexRemovedExplicit += value;
            remove => VertexRemovedExplicit -= value;
        }
        event Event<IEdge> IGraph.EdgeAdded
        {
            add => EdgeAddedExplicit += value;
            remove => EdgeAddedExplicit -= value;
        }
        event Event<IEdge> IGraph.EdgeRemoved
        {
            add => EdgeRemovedExplicit += value;
            remove => EdgeRemovedExplicit -= value;
        }

        protected GraphBase()
        {
            VerticesSet = new HashSet<TVertexBase>();
            EdgesSet = new HashSet<TEdgeBase>();

            Vertices = new ReadOnlyHashSet<TVertexBase>(VerticesSet);
            Edges = new ReadOnlyHashSet<TEdgeBase>(EdgesSet);
        }
        
        public abstract void Clear();
    }
}