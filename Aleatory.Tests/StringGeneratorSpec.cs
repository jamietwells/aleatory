using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Aleatory.Tests
{
    public class StringGeneratorSpec
    {
        private readonly StringGenerator _generator;
        private readonly Random _random;

        public StringGeneratorSpec()
        {
            _generator = new DataGenerators(new Random(12345))
                .StringGenerator()
                .LengthBetween(10, 100);

            _random = new Random();
        }

        [Fact]
        public void CanGenerateSingleValue()
        {
            var min = 'a';
            var max = 'z';

            var minLength = 10;
            var maxLengh = 100;

            var gen = _generator
                .AddCharacterGroup()
                .CharactersBetween(min, max)
                .CompleteGroup()
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
        public void StringContainsRandomCharacters()
        {
            var ranges = new (char Min, char Max)[] { ('a', 'd'), ('g', 'n'), ('q', 'y'), ('A', 'G'), ('K', 'U') };

            var minLength = 1000;
            var maxLengh = 10000;

            var gen = _generator;

            foreach (var range in ranges)
            {
                gen = gen
                    .AddCharacterGroup()
                    .CharactersBetween(range.Min, range.Max)
                    .CompleteGroup();
            }

            gen = gen
                .LengthBetween(minLength, maxLengh);

            var value = gen.Single();

            value.Distinct().Should().BeEquivalentTo(ranges.SelectMany(r => Enumerable.Range(r.Min, r.Max - r.Min).Select(c => (char)c)));
        }

        [Fact]
        public void CharacterGroupsChosenRandomly()
        {
            var ranges = new (char Min, char Max)[] { ('a', 'd'), ('g', 'n'), ('q', 'y'), ('A', 'G'), ('K', 'U') };

            var minLength = 10;
            var maxLengh = 100;

            var gen = _generator;

            foreach (var range in ranges)
            {
                gen = gen
                    .AddCharacterGroup()
                    .CharactersBetween(range.Min, range.Max)
                    .CompleteGroup();
            }

            gen = gen
                .LengthBetween(minLength, maxLengh);


            var strings = Enumerable.Range(0, 1000)
                .Select(_ => gen.Single());

            var position = _random.Next(0, minLength);

            var set = new HashSet<char>(strings.Select(s => s[position]));
            var rangeGroups = ranges.Select(r => Enumerable.Range(r.Min, r.Max - r.Min).Select(c => (char)c));

            foreach(var range in rangeGroups)
                set.Should().IntersectWith(range);
        }

        [Fact]
        public void CanTakeManyValues()
        {
            var count = _random.Next(100, 200);

            var chars = _generator
                .AddCharacterGroup()
                .CharactersBetween('a', 'z')
                .CompleteGroup()
                .Many()
                .Take(count);

            chars.Should().HaveCount(count);
        }

        [Fact]
        public void ValuesWillGenerateBetweenTheSpecifiedValues()
        {
            var min = (char)_random.Next('a', 'm');
            var max = (char)_random.Next('n', 'z');

            var strings = _generator
                .AddCharacterGroup()
                .CharactersBetween(min, max)
                .CompleteGroup()
                .Many()
                .Take(10000)
                .Distinct();

            strings.SelectMany(s => s.ToCharArray()).Distinct().Should().BeEquivalentTo(Enumerable.Range(min, (max - min)).Select(Convert.ToChar));
        }

        [Fact]
        public void InclusiveLower_Includes_LowerBound()
        {
            var min = (char)_random.Next(1, 10);
            var strings = _generator
                .AddCharacterGroup()
                .CharactersBetween(min, (char)20)
                .CharactersInclusiveLower()
                .CompleteGroup()
                .Many()
                .Take(10000);

            strings.SelectMany(s => s.ToCharArray()).Should().Contain(min);
        }

        [Fact]
        public void InclusiveUpper_Includes_UpperBound()
        {
            var max = (char)_random.Next(10, 20);
            var strings = _generator
                .AddCharacterGroup()
                .CharactersBetween((char)1, max)
                .CharactersInclusiveUpper()
                .CompleteGroup()
                .Many()
                .Take(10000);

            strings.SelectMany(s => s.ToCharArray()).Should().Contain(max);
        }

        [Fact]
        public void ExclusiveLower_DoesNotInclude_LowerBound()
        {
            var min = (char)_random.Next(1, 10);
            var strings = _generator
                .AddCharacterGroup()
                .CharactersBetween(min, (char)20)
                .CharactersExclusiveLower()
                .CompleteGroup()
                .Many()
                .Take(10000);

            strings.SelectMany(s => s.ToCharArray()).Should().NotContain(min);
        }

        [Fact]
        public void ExclusiveUpper_DoesNotInclude_UpperBound()
        {
            var max = (char)_random.Next(10, 20);
            var strings = _generator
                .AddCharacterGroup()
                .CharactersBetween((char)1, max)
                .CharactersExclusiveUpper()
                .CompleteGroup()
                .Many()
                .Take(10000);

            strings.SelectMany(s => s.ToCharArray()).Should().NotContain(max);
        }

        [Fact]
        public void LengthInclusiveLower_Includes_LowerBound()
        {
            var min = (char)_random.Next(1, 10);
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
            var max = (char)_random.Next(10, 20);
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
            var min = (char)_random.Next(1, 10);
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
            var max = (char)_random.Next(10, 20);
            var strings = _generator
                .LengthBetween((char)1, max)
                .LengthExclusiveUpper()
                .Many()
                .Take(10000);

            strings.Select(s => s.Length).Max().Should().BeLessThan(max);
        }
    }
}
