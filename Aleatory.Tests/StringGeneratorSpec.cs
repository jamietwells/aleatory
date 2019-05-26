using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Xunit;
using Xunit.Sdk;

namespace Aleatory.Tests
{
    public class StringGeneratorSpec
    {
        private readonly StringGenerator _generator;

        public StringGeneratorSpec()
        {
            _generator = new DataGenerators(new Random(12345))
                .StringGenerator()
                .LengthBetween(10, 100);
        }

        [Fact]
        public void CanGenerateSingleValue()
        {
            var min = 'a';
            var max = 'z';

            var minLength = 10;
            var maxLengh = 100;

            var gen = _generator
                .CharactersBetween(min, max)
                .LengthBetween(minLength, maxLengh);

            var strings = Enumerable.Repeat<Func<string>>(gen.Single, 10000)
                .Select(func => func())
                .ToArray();

            strings.Count().Should().Be(10000);
            var lengths = strings.Select(s => s.Length);

            lengths.Min().Should().BeGreaterOrEqualTo(minLength);
            lengths.Max().Should().BeLessOrEqualTo(maxLengh);

        }

        [Fact]
        public void CanTakeManyValues()
        {
            var count = new Random().Next(100, 200);

            var chars = _generator
                .CharactersBetween('a', 'z')
                .Many()
                .Take(count);

            chars.Should().HaveCount(count);
        }

        [Fact]
        public void ValuesWillGenerateBetweenTheSpecifiedValues()
        {
            var min = (char)new Random().Next('a', 'm');
            var max = (char)new Random().Next('n', 'z');
            
            var strings = _generator
                .CharactersBetween(min, max)
                .Many()
                .Take(10000)
                .Distinct();

            strings.SelectMany(s => s.ToCharArray()).Distinct().Should().BeEquivalentTo(Enumerable.Range(min, (max - min)).Select(Convert.ToChar));
        }

        [Fact]
        public void InclusiveLower_Includes_LowerBound()
        {
            var min = (char)new Random().Next(1, 10);
            var strings = _generator
                .CharactersBetween(min, (char)20)
                .CharactersInclusiveLower()
                .Many()
                .Take(10000);

            strings.SelectMany(s => s.ToCharArray()).Should().Contain(min);
        }

        [Fact]
        public void InclusiveUpper_Includes_UpperBound()
        {
            var max = (char)new Random().Next(10, 20);
            var strings = _generator
                .CharactersBetween((char)1, max)
                .CharactersInclusiveUpper()
                .Many()
                .Take(10000);

            strings.SelectMany(s => s.ToCharArray()).Should().Contain(max);
        }

        [Fact]
        public void ExclusiveLower_DoesNotInclude_LowerBound()
        {
            var min = (char)new Random().Next(1, 10);
            var strings = _generator
                .CharactersBetween(min, (char)20)
                .CharactersExclusiveLower()
                .Many()
                .Take(10000);

            strings.SelectMany(s => s.ToCharArray()).Should().NotContain(min);
        }

        [Fact]
        public void ExclusiveUpper_DoesNotInclude_UpperBound()
        {
            var max = (char)new Random().Next(10, 20);
            var strings = _generator
                .CharactersBetween((char)1, max)
                .CharactersExclusiveUpper()
                .Many()
                .Take(10000);

            strings.SelectMany(s => s.ToCharArray()).Should().NotContain(max);
        }

                [Fact]
        public void LengthInclusiveLower_Includes_LowerBound()
        {
            var min = (char)new Random().Next(1, 10);
            var strings = _generator
                .LengthBetween(min, (char)20)
                .LengthInclusiveLower()
                .Many()
                .Take(10000);

            strings.Select(s => s.Length).Min().Should().BeGreaterOrEqualTo(min);
        }

        [Fact]
        public void LengthInclusiveUpper_Includes_UpperBound()
        {
            var max = (char)new Random().Next(10, 20);
            var strings = _generator
                .LengthBetween((char)1, max)
                .LengthInclusiveUpper()
                .Many()
                .Take(10000);

            strings.Select(s => s.Length).Max().Should().BeLessOrEqualTo(max);
        }

        [Fact]
        public void LengthExclusiveLower_DoesNotInclude_LowerBound()
        {
            var min = (char)new Random().Next(1, 10);
            var strings = _generator
                .LengthBetween(min, (char)20)
                .LengthExclusiveLower()
                .Many()
                .Take(10000);

            strings.Select(s => s.Length).Min().Should().BeGreaterThan(min);
        }

        [Fact]
        public void LengthExclusiveUpper_DoesNotInclude_UpperBound()
        {
            var max = (char)new Random().Next(10, 20);
            var strings = _generator
                .LengthBetween((char)1, max)
                .LengthExclusiveUpper()
                .Many()
                .Take(10000);

            strings.Select(s => s.Length).Max().Should().BeLessThan(max);
        }
    }
}
