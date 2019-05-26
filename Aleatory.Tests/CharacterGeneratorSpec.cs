using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Aleatory.Tests
{
    public class CharacterGeneratorSpec
    {
        private readonly CharacterGenerator _generator;

        public CharacterGeneratorSpec()
        {
            _generator = new DataGenerators(new Random(12345))
                .CharacterGenerator();
        }

        [Fact]
        public void CanGenerateSingleValue()
        {
            var min = 'a';
            var max = 'z';

            var gen = _generator
                .Between(min, max);

            var chars = Enumerable.Repeat<Func<char>>(gen.Single, 10000)
                .Select(func => func())
                .ToArray();

            chars.Count().Should().Be(10000);
            chars.Distinct().Should().BeEquivalentTo(Enumerable.Range(min, (max - min)).Select(Convert.ToChar));
        }

        [Fact]
        public void CanTakeManyValues()
        {
            var count = new Random().Next(100, 200);

            var chars = _generator
                .Between('a', 'z')
                .Many()
                .Take(count);

            chars.Should().HaveCount(count);
        }

        [Fact]
        public void ValuesWillGenerateBetweenTheSpecifiedValues()
        {
            var min = (char)new Random().Next('a', 'm');
            var max = (char)new Random().Next('n', 'z');
            
            var numbers = _generator
                .Between(min, max)
                .Many()
                .Take(10000)
                .Distinct();

            numbers.Should().HaveCountLessOrEqualTo(max - min);
        }

        [Fact]
        public void InclusiveLower_Includes_LowerBound()
        {
            var min = (char)new Random().Next(1, 10);
            var numbers = _generator
                .Between(min, (char)20)
                .InclusiveLower()
                .Many()
                .Take(10000);

            numbers.Should().Contain(min);
        }

        [Fact]
        public void InclusiveUpper_Includes_UpperBound()
        {
            var max = (char)new Random().Next(10, 20);
            var numbers = _generator
                .Between((char)1, max)
                .InclusiveUpper()
                .Many()
                .Take(10000);

            numbers.Should().Contain(max);
        }

        [Fact]
        public void ExclusiveLower_DoesNotInclude_LowerBound()
        {
            var min = (char)new Random().Next(1, 10);
            var numbers = _generator
                .Between(min, (char)20)
                .ExclusiveLower()
                .Many()
                .Take(10000);

            numbers.Should().NotContain(min);
        }

        [Fact]
        public void ExclusiveUpper_DoesNotInclude_UpperBound()
        {
            var max = (char)new Random().Next(10, 20);
            var numbers = _generator
                .Between((char)1, max)
                .ExclusiveUpper()
                .Many()
                .Take(10000);

            numbers.Should().NotContain(max);
        }
    }
}
