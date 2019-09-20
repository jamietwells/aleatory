using System.Linq;
using FluentAssertions;
using Xunit;

namespace Aleatory.Tests
{
    public class ExtensionTests
    {
        [Fact]
        public void YieldExtensionReturnsSingleItemEnumerated()
        {
            var item = new { A = "A", B = 1 };

            item.Yield().Should().BeEquivalentTo(new[] { item });
        }

        [Fact]
        public void RandomElementGivesASingleRandomItemFromEnumerable()
        {
            var items = Enumerable.Range(1, 10)
                .Select(i => new { A = i.ToString(), B = i })
                .ToArray();

            Enumerable.Range(0, 1000).Select(_ => items.RandomElement()).Distinct()
                .Should().BeEquivalentTo(items);
        }
    }
}
