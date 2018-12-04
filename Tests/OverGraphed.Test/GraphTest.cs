using NUnit.Framework;
using OverGraphed.Test.Base;
using OverGraphed.Test.Utils;

namespace OverGraphed.Test
{
    [TestFixture]
    public class GraphTest : GraphTestBase
    {
        protected override IGraph<ITestVertex, ITestEdge> GetObservedGraph(Graph<ITestVertex, ITestEdge> graph)
        {
            return graph;
        }
    }
}