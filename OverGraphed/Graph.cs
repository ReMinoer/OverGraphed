using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace OverGraphed
{
    public class Graph<TVertexBase, TEdgeBase> : IWritableGraph<TVertexBase, TEdgeBase>
        where TVertexBase : Vertex<TVertexBase, TEdgeBase>
        where TEdgeBase : class, IEdge<TVertexBase, TEdgeBase>
    {
        private readonly List<TVertexBase> _vertices;
        private readonly List<TEdgeBase> _edges;
        private readonly ReadOnlyCollection<TVertexBase> _readOnlyVertices;
        private readonly ReadOnlyCollection<TEdgeBase> _readOnlyEdges;
        public IEnumerable<TVertexBase> Vertices => _readOnlyVertices;
        public IEnumerable<TEdgeBase> Edges => _readOnlyEdges;

        public Graph()
        {
            _vertices = new List<TVertexBase>();
            _edges = new List<TEdgeBase>();

            _readOnlyVertices = new ReadOnlyCollection<TVertexBase>(_vertices);
            _readOnlyEdges = new ReadOnlyCollection<TEdgeBase>(_edges);
        }

        public TEdgeBase this[TVertexBase start, TVertexBase end]
        {
            get
            {
                return _vertices.Contains(start)
                    ? start.Edges.FirstOrDefault(x => x.End == end)
                    : null;
            }
        }

        public virtual void AddVertex(TVertexBase vertex)
        {
            if (_vertices.Contains(vertex))
                throw new ArgumentException("Vertex provided is already in the graph !");

            _vertices.Add(vertex);
        }

        public virtual void RemoveVertex(TVertexBase vertex)
        {
            if (!_vertices.Contains(vertex))
                throw new ArgumentException("Vertex provided is not in the graph !");

            ClearEdges(vertex);
            _vertices.Remove(vertex);
        }

        public virtual void ClearVertices()
        {
            _edges.Clear();
            _vertices.Clear();
        }

        public void AddEdge<TStart, TEnd, TEdge>(TStart from, TEnd to, TEdge edge)
            where TStart : TVertexBase
            where TEnd : TVertexBase
            where TEdge : Edge<TStart, TEnd, TVertexBase, TEdgeBase>, TEdgeBase
        {
            if (ContainsEdge(from, to))
                throw new ArgumentException("Edge already exist !");

            edge.Start = from;
            edge.End = to;

            _edges.Add(edge);
            edge.Start.AddEdge(edge);
            edge.End.AddEdge(edge);
        }

        public void RemoveEdge(TVertexBase from, TVertexBase to)
        {
            if (!ContainsEdge(from, to))
                throw new ArgumentException("Edge doesn't exist !");

            TEdgeBase edge = from.Edges.First(x => x.End == to);

            edge.Start.RemoveEdge(edge);
            edge.End.RemoveEdge(edge);
            _edges.Remove(edge);
        }

        public void RemoveEdge(TEdgeBase edge)
        {
            if (!ContainsEdge(edge.Start, edge.End))
                throw new ArgumentException("Edge doesn't exist !");

            edge.Start.RemoveEdge(edge);
            edge.End.RemoveEdge(edge);
            _edges.Remove(edge);
        }

        public bool ContainsEdge(TVertexBase from, TVertexBase to)
        {
            if (!_vertices.Contains(from))
                throw new ArgumentException("Start vertex provided is not in the graph !");

            if (!_vertices.Contains(to))
                throw new ArgumentException("End vertex provided is not in the graph !");

            return _edges.Any(x => x.Start == from && x.End == to);
        }

        public virtual void ClearEdges(TVertexBase vertex)
        {
            if (!_vertices.Contains(vertex))
                throw new ArgumentException("Vertex provided is not in the graph !");

            vertex.ClearEdges();

            foreach (TEdgeBase edge in vertex.Edges)
                _edges.Remove(edge);
        }

        public virtual void ClearEdges()
        {
            foreach (TVertexBase vertex in _vertices)
                vertex.ClearEdges();

            _edges.Clear();
        }
    }
}