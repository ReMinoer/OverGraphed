using System;
using System.Collections.Generic;

namespace OverGraphed
{
    public class ReadOnlyGraph<TVertexBase, TEdgeBase> : IGraph<TVertexBase, TEdgeBase>, IDisposable
        where TVertexBase : class, IVertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        private readonly IGraph<TVertexBase, TEdgeBase> _graph;

        public IEnumerable<TVertexBase> Vertices => _graph.Vertices;
        public IEnumerable<TEdgeBase> Edges => _graph.Edges;
        IEnumerable<IVertex> IGraph.Vertices => Vertices;
        IEnumerable<IEdge> IGraph.Edges => Edges;

        public event Event<TVertexBase> VertexAdded;
        public event Event<TVertexBase> VertexRemoved;
        public event Event<TEdgeBase> EdgeAdded;
        public event Event<TEdgeBase> EdgeRemoved;
        public event Event Cleared;

        private event Event<IVertex> VertexAddedExplicit;
        private event Event<IVertex> VertexRemovedExplicit;
        private event Event<IEdge> EdgeAddedExplicit;
        private event Event<IEdge> EdgeRemovedExplicit;

        event Event<IVertex> IGraph.VertexAdded
        {
            add => VertexAddedExplicit += value;
            remove => VertexAddedExplicit += value;
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

        public ReadOnlyGraph(IGraph<TVertexBase, TEdgeBase> graph)
        {
            _graph = graph ?? throw new NullReferenceException(nameof(graph));

            _graph.VertexAdded += OnVertexAdded;
            _graph.VertexRemoved += OnVertexRemoved;
            _graph.EdgeAdded += OnEdgeAdded;
            _graph.EdgeRemoved += OnEdgeRemoved;
            _graph.Cleared += OnCleared;

            IGraph graphBase = _graph;
            graphBase.VertexAdded += OnVertexAdded;
            graphBase.VertexRemoved += OnVertexRemoved;
            graphBase.EdgeAdded += OnEdgeAdded;
            graphBase.EdgeRemoved += OnEdgeRemoved;
        }

        private void OnVertexAdded(object sender, TVertexBase e) => VertexAdded?.Invoke(this, e);
        private void OnVertexRemoved(object sender, TVertexBase e) => VertexRemoved?.Invoke(this, e);
        private void OnEdgeAdded(object sender, TEdgeBase e) => EdgeAdded?.Invoke(this, e);
        private void OnEdgeRemoved(object sender, TEdgeBase e) => EdgeRemoved?.Invoke(this, e);
        private void OnCleared(object sender) => Cleared?.Invoke(this);

        private void OnVertexAdded(object sender, IVertex e) => VertexAddedExplicit?.Invoke(this, e);
        private void OnVertexRemoved(object sender, IVertex e) => VertexRemovedExplicit?.Invoke(this, e);
        private void OnEdgeAdded(object sender, IEdge e) => EdgeAddedExplicit?.Invoke(this, e);
        private void OnEdgeRemoved(object sender, IEdge e) => EdgeRemovedExplicit?.Invoke(this, e);

        public void Dispose()
        {
            _graph.VertexAdded -= OnVertexAdded;
            _graph.VertexRemoved -= OnVertexRemoved;
            _graph.EdgeAdded -= OnEdgeAdded;
            _graph.EdgeRemoved -= OnEdgeRemoved;
            _graph.Cleared -= OnCleared;

            IGraph graphBase = _graph;
            graphBase.VertexAdded -= OnVertexAdded;
            graphBase.VertexRemoved -= OnVertexRemoved;
            graphBase.EdgeAdded -= OnEdgeAdded;
            graphBase.EdgeRemoved -= OnEdgeRemoved;
        }
    }
}