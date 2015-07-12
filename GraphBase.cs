using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Diese.Graph
{
    public class GraphBase<TGraph, TVertex, TEdge, TVisitor> : IGraph<TGraph, TVertex, TEdge, TVisitor>
        where TGraph : GraphBase<TGraph, TVertex, TEdge, TVisitor>
        where TVertex : VertexBase<TGraph, TVertex, TEdge, TVisitor>
        where TEdge : EdgeBase<TGraph, TVertex, TEdge, TVisitor>
        where TVisitor : VisitorBase<TGraph, TVertex, TEdge, TVisitor>
    {
        private readonly List<TVertex> _vertices;
        private readonly List<TEdge> _edges;
        private readonly IReadOnlyCollection<TVertex> _readOnlyVertices;
        private readonly IReadOnlyCollection<TEdge> _readOnlyEdges;

        public IReadOnlyCollection<TVertex> Vertices
        {
            get { return _readOnlyVertices; }
        }

        public IReadOnlyCollection<TEdge> Edges
        {
            get { return _readOnlyEdges; }
        }

        public GraphBase()
        {
            _vertices = new List<TVertex>();
            _edges = new List<TEdge>();

            _readOnlyVertices = _vertices.AsReadOnly();
            _readOnlyEdges = _edges.AsReadOnly();
        }

        public TEdge this[TVertex start, TVertex end]
        {
            get { return _vertices.Contains(start) ? start.Edges.FirstOrDefault(x => x.End == end) : null; }
        }

        public virtual void AddVertex(TVertex vertex)
        {
            if (_vertices.Contains(vertex))
                throw new ArgumentException("Vertex provided is already in the graph !");

            _vertices.Add(vertex);
        }

        public virtual void RemoveVertex(TVertex vertex)
        {
            if (!_vertices.Contains(vertex))
                throw new ArgumentException("Vertex provided is not in the graph !");

            ClearEdges(vertex);
            _vertices.Remove(vertex);
        }

        public virtual bool ContainsVertex(TVertex vertex)
        {
            return _vertices.Contains(vertex);
        }

        public virtual void ClearVertices()
        {
            _edges.Clear();
            _vertices.Clear();
        }

        public void AddEdge(ref TEdge edge)
        {
            if (!_vertices.Contains(edge.Start))
                throw new ArgumentException("Start vertex provided is not in the graph !");

            if (!_vertices.Contains(edge.End))
                throw new ArgumentException("End vertex provided is not in the graph !");

            foreach (TEdge otherEdge in _edges)
            {
                if (!edge.Equals(otherEdge))
                    continue;

                edge = otherEdge;
                return;
            }

            _edges.Add(edge);
            edge.Start.AddEdge(edge);
            edge.End.AddEdge(edge);
        }

        public void RemoveEdge(ref TEdge edge)
        {
            if (!_vertices.Contains(edge.Start))
                throw new ArgumentException("Start vertex provided is not in the graph !");

            if (!_vertices.Contains(edge.End))
                throw new ArgumentException("End vertex provided is not in the graph !");

            bool exists = false;
            foreach (TEdge otherEdge in _edges)
                if (edge.Equals(otherEdge))
                {
                    edge = otherEdge;
                    exists = true;
                    break;
                }

            if (!exists)
                throw new ArgumentException("This dependency relation doesn't exists !");

            _edges.Remove(edge);
            edge.Start.RemoveEdge(edge);
            edge.End.RemoveEdge(edge);
        }

        public bool ContainsEdge(ref TEdge edge)
        {
            foreach (TEdge otherEdge in _edges)
                if (edge.Equals(otherEdge))
                {
                    edge = otherEdge;
                    return true;
                }

            return false;
        }

        public virtual void ClearEdges(TVertex vertex)
        {
            if (!_vertices.Contains(vertex))
                throw new ArgumentException("Vertex provided is not in the graph !");

            foreach (TEdge edge in vertex.Edges)
                _edges.Remove(edge);

            vertex.ClearEdges();
        }

        public virtual void ClearEdges()
        {
            _edges.Clear();

            foreach (TVertex vertex in _vertices)
                vertex.ClearEdges();
        }

        public IEnumerator<TVertex> GetEnumerator()
        {
            return _vertices.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}