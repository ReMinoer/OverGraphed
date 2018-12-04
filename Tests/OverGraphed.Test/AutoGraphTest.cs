using NUnit.Framework;
using OverGraphed.Test.Base;
using OverGraphed.Test.Utils;

namespace OverGraphed.Test
{
    [TestFixture]
    public class AutoGraphTest : AutoGraphTestBase
    {
        protected override IGraph<ITestVertex, ITestEdge> GetObservedGraph(AutoGraph<ITestVertex, ITestEdge> graph)
        {
            return graph;
        }
    }
}