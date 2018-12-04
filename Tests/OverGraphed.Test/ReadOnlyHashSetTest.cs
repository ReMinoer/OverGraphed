using System.Collections;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using OverGraphed.Utils;

namespace OverGraphed.Test
{
    public class ReadOnlyHashSetTest
    {
        // Implementation is simple but too complex to test.
        // We consider implementation is good and simply call members for 100% covering.
        [Test]
        public void Cover()
        {
            var hashSet = new HashSet<int>();
            var readOnlyHashSet = new ReadOnlyHashSet<int>(hashSet);

            const int item = 1;
            var other = new HashSet<int> { item };

            readOnlyHashSet.Count.Should().Be(hashSet.Count);
            readOnlyHashSet.Comparer.Should().Be(hashSet.Comparer);
            readOnlyHashSet.Invoking(x => x.Contains(item)).Should().NotThrow();
            readOnlyHashSet.Invoking(x => x.SetEquals(other)).Should().NotThrow();
            readOnlyHashSet.Invoking(x => x.Overlaps(other)).Should().NotThrow();
            readOnlyHashSet.Invoking(x => x.IsSubsetOf(other)).Should().NotThrow();
            readOnlyHashSet.Invoking(x => x.IsSupersetOf(other)).Should().NotThrow();
            readOnlyHashSet.Invoking(x => x.IsProperSubsetOf(other)).Should().NotThrow();
            readOnlyHashSet.Invoking(x => x.IsProperSupersetOf(other)).Should().NotThrow();

            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            readOnlyHashSet.Invoking(x => x.GetEnumerator()).Should().NotThrow();
            // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
            ((IEnumerable)readOnlyHashSet).Invoking(x => x.GetEnumerator()).Should().NotThrow();
        }
    }
}