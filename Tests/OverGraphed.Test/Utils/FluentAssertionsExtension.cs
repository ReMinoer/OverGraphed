using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FluentAssertions.Collections;
using FluentAssertions.Events;
using FluentAssertions.Execution;

namespace OverGraphed.Test.Utils
{
    static public class FluentAssertionsExtension
    {
        static public AndConstraint<GenericCollectionAssertions<T>> Contain<T>(this GenericCollectionAssertions<T> should, params T[] items)
        {
            using (new AssertionScope())
            using (IEnumerator<T> enumerator = should.Subject.GetEnumerator())
            {
                if (!enumerator.MoveNext())
                    return new AndConstraint<GenericCollectionAssertions<T>>(should);

                AndConstraint<GenericCollectionAssertions<T>> result = should.Contain(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    result = result.And.Contain(enumerator.Current);
                }

                return result;
            }
        }

        static public AndConstraint<GenericCollectionAssertions<T>> ContainOnly<T>(this GenericCollectionAssertions<T> should, params T[] items)
        {
            using (new AssertionScope())
                return items.Aggregate(should.HaveCount(items.Length), (current, item) => current.And.Contain(item));
        }

        static public AndConstraint<GenericCollectionAssertions<T>> ContainOnlyAndInOrder<T>(this GenericCollectionAssertions<T> should, params T[] items)
        {
            return should.HaveCount(items.Length).And.ContainInOrder(items);
        }

        static public IEventRecorder WithArgs(this IEventRecorder eventRecorder, params object[] argsInstances)
        {
            using (new AssertionScope())
            {
                foreach (object argsInstance in argsInstances)
                    eventRecorder.WithArgs<object>(x => x == argsInstance);
            }
            return eventRecorder;
        }
    }
}