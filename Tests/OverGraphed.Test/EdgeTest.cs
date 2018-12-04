using System;
using FluentAssertions;
using NUnit.Framework;
using OverGraphed.Test.Utils;

namespace OverGraphed.Test
{
    [TestFixture]
    public class EdgeTest
    {
        [Test]
        public void VertexBaseConstructorTest()
        {
            var edge = new Edge<TestVertex>();

            edge.Start.Should().BeNull();
            edge.End.Should().BeNull();
        }

        [Test]
        public void VertexBaseConstructorWithOwnerTest()
        {
            var owner = new Edge<TestVertex>();
            var edge = new Edge<TestVertex>(owner);

            edge.Start.Should().BeNull();
            edge.End.Should().BeNull();
        }

        public class TestVertex : SimpleDirectedVertex<TestVertex, Edge<TestVertex>>
        {
        }

        public class BothBaseEdge : Edge<ITestVertex, ITestEdge>, ITestEdge
        {
            public BothBaseEdge()
            {
            }

            public BothBaseEdge(ITestEdge owner)
                : base(owner)
            {
            }
        }

        public class InvalidBothBaseEdge : Edge<ITestVertex, ITestEdge>
        {
        }

        [Test]
        public void BothBaseConstructorTest()
        {
            var edge = new BothBaseEdge();

            edge.Start.Should().BeNull();
            edge.End.Should().BeNull();
        }

        [Test]
        public void BothBaseConstructorWithOwnerTest()
        {
            var owner = new BothBaseEdge();
            var edge = new BothBaseEdge(owner);

            edge.Start.Should().BeNull();
            edge.End.Should().BeNull();
        }

        [Test]
        public void InvalidBothBaseConstructorTest()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action constructor = () => new InvalidBothBaseEdge();
            constructor.Should().Throw<InvalidOperationException>();
        }

        public class BothBaseWithStartEndEdge : Edge<ITestVertex, ITestVertex, ITestVertex, ITestEdge>, ITestEdge
        {
            public BothBaseWithStartEndEdge()
            {
            }

            public BothBaseWithStartEndEdge(ITestEdge owner)
                : base(owner)
            {
            }
        }

        public class InvalidBothBaseWithStartEndEdge : Edge<ITestVertex, ITestEdge>
        {
        }

        [Test]
        public void BothBaseWithStartEndConstructorTest()
        {
            var edge = new BothBaseWithStartEndEdge();

            edge.Start.Should().BeNull();
            edge.End.Should().BeNull();
        }

        [Test]
        public void BothBaseWithStartEndConstructorWithOwnerTest()
        {
            var owner = new BothBaseWithStartEndEdge();
            var edge = new BothBaseWithStartEndEdge(owner);

            edge.Start.Should().BeNull();
            edge.End.Should().BeNull();
        }

        [Test]
        public void InvalidBothBaseWithStartEndConstructorTest()
        {
            // ReSharper disable once ObjectCreationAsStatement
            Action constructor = () => new InvalidBothBaseWithStartEndEdge();
            constructor.Should().Throw<InvalidOperationException>();
        }
    }
}