using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Aleatory.Tests
{
    public class IntegerGeneratorSpec
    {
        private readonly IntegerGenerator _generator;

        public IntegerGeneratorSpec()
        {
            _generator = new DataGenerators(new Random(12345))
                .IntegerGenerator();
        }

        [Fact]
        public void CanGenerateSingleValue()
        {
            var min = 1;
            var max = 10;

            var gen = _generator
                .Between(min, max);

            var nums = Enumerable.Repeat<Func<int>>(gen.Single, 10000)
                .Select(func => func())
                .ToArray();

            nums.Count().Should().Be(10000);
            nums.Distinct().Should().BeEquivalentTo(Enumerable.Range(min, max - 1));
        }

        [Fact]
        public void CanTakeManyValues()
        {
            var count = new Random().Next(100, 200);

            var numbers = _generator
                .Between(1, 10)
                .Many()
                .Take(count);

            numbers.Should().HaveCount(count);
        }

        [Fact]
        public void ValuesWillGenerateBetweenTheSpecifiedValues()
        {
            var max = new Random().Next(10, 20);
            var numbers = _generator
                .Between(1, max)
                .Many()
                .Take(10000)
                .Distinct();

            numbers.Should().HaveCountLessThan(max);
        }

        [Fact]
        public void InclusiveLower_Includes_LowerBound()
        {
            var min = new Random().Next(1, 10);
            var numbers = _generator
                .Between(min, 20)
                .InclusiveLower()
                .Many()
                .Take(10000);

            numbers.Should().Contain(min);
        }

        [Fact]
        public void InclusiveUpper_Includes_UpperBound()
        {
            var max = new Random().Next(10, 20);
            var numbers = _generator
                .Between(1, max)
                .InclusiveUpper()
                .Many()
                .Take(10000);

            numbers.Should().Contain(max);
        }

        [Fact]
        public void ExclusiveLower_DoesNotInclude_LowerBound()
        {
            var min = new Random().Next(1, 10);
            var numbers = _generator
                .Between(min, 20)
                .ExclusiveLower()
                .Many()
                .Take(10000);

            numbers.Should().NotContain(min);
        }

        [Fact]
        public void ExclusiveUpper_DoesNotInclude_UpperBound()
        {
            var max = new Random().Next(10, 20);
            var numbers = _generator
                .Between(1, max)
                .ExclusiveUpper()
                .Many()
                .Take(10000);

            numbers.Should().NotContain(max);
        }
    }
}
