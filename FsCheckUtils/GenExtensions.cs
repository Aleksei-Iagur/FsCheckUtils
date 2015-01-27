﻿using System;
using System.Collections.Generic;
using System.Linq;
using FsCheck;
using FsCheck.Fluent;

namespace FsCheckUtils
{
    public static class GenExtensions
    {
        /// <summary>
        /// A generator that picks a given number of elements from a list, randomly
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list</typeparam>
        /// <param name="n">The number of items to pick from the list</param>
        /// <param name="l">The list of elements to pick from</param>
        /// <returns>A generator that generates a given number of elements from a list, randomly</returns>
        public static Gen<List<T>> PickValues<T>(int n, params T[] l)
        {
            if (n < 0 || n > l.Length) throw new ArgumentOutOfRangeException("n");

            Func<List<T>, List<int>, List<T>> removeItems = (b, idxs) =>
            {
                foreach (var idx in idxs) b.RemoveAt(idx % b.Count);
                return b;
            };

            var numItemsToRemove = l.Length - n;

            return
                from idxs in Any.IntBetween(0, l.Length * 10).MakeListOfLength(numItemsToRemove)
                select removeItems(new List<T>(l), idxs);
        }

        /// <summary>
        /// A generator that picks a given number of elements from a list, randomly
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list</typeparam>
        /// <param name="n">The number of items to pick from the list</param>
        /// <param name="l">The list of elements to pick from</param>
        /// <returns>A generator that generates a given number of elements from a list, randomly</returns>
        public static Gen<List<T>> PickValues<T>(int n, IEnumerable<T> l)
        {
            return PickValues(n, l.ToArray());
        }

        /// <summary>
        /// A generator that picks a given number of elements from a list of generators, randomly
        /// </summary>
        /// <typeparam name="T">The type generated by the generators in the list of generators</typeparam>
        /// <param name="n">The number of items to pick from the list</param>
        /// <param name="gs">The list of generators to pick from</param>
        /// <returns>A generator that generates a given number of elements from a list of generators, randomly</returns>
        public static Gen<List<T>> PickGenerators<T>(int n, params Gen<T>[] gs)
        {
            var genIdxs = PickValues(n, Enumerable.Range(0, gs.Length));
            return genIdxs.SelectMany(idxs => Any.SequenceOf(idxs.Select(x => gs[x])));
        }

        /// <summary>
        /// A generator that picks a given number of elements from a list of generators, randomly
        /// </summary>
        /// <typeparam name="T">The type generated by the generators in the list of generators</typeparam>
        /// <param name="n">The number of items to pick from the list</param>
        /// <param name="gs">The list of generators to pick from</param>
        /// <returns>A generator that generates a given number of elements from a list of generators, randomly</returns>
        public static Gen<List<T>> PickGenerators<T>(int n, IEnumerable<Gen<T>> gs)
        {
            return PickGenerators(n, gs.ToArray());
        }

        /// <summary>
        /// A generator that picks a random number of elements from a list
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list</typeparam>
        /// <param name="l">The list of elements to pick from</param>
        /// <returns>A generator that generates a random number of elements from a list</returns>
        public static Gen<List<T>> SomeOfValues<T>(params T[] l)
        {
            return Gen.choose(0, l.Length).SelectMany(n => PickValues(n, l));
        }

        /// <summary>
        /// A generator that picks a random number of elements from a list
        /// </summary>
        /// <typeparam name="T">The type of the elements in the list</typeparam>
        /// <param name="l">The list of elements to pick from</param>
        /// <returns>A generator that generates a random number of elements from a list</returns>
        public static Gen<List<T>> SomeOfValues<T>(IEnumerable<T> l)
        {
            return SomeOfValues(l.ToArray());
        }

        /// <summary>
        /// A generator that picks a random number of elements from a list of generators
        /// </summary>
        /// <typeparam name="T">The type generated by the generators in the list of generators</typeparam>
        /// <param name="gs">The list of generators to pick from</param>
        /// <returns>A generator that generates a random number of elements from a list of generators</returns>
        public static Gen<List<T>> SomeOfGenerators<T>(params Gen<T>[] gs)
        {
            return Gen.choose(0, gs.Length).SelectMany(n => PickGenerators(n, gs));
        }

        /// <summary>
        /// A generator that picks a random number of elements from a list of generators
        /// </summary>
        /// <typeparam name="T">The type generated by the generators in the list of generators</typeparam>
        /// <param name="gs">The list of generators to pick from</param>
        /// <returns>A generator that generates a random number of elements from a list of generators</returns>
        public static Gen<List<T>> SomeOfGenerators<T>(IEnumerable<Gen<T>> gs)
        {
            return SomeOfGenerators(gs.ToArray());
        }

        /// <summary>
        /// Generates a numerical character
        /// </summary>
        /// <returns>A generator that generates a numerical character</returns>
        public static Gen<char> NumChar()
        {
            return from n in Gen.choose('0', '9')
                   select Convert.ToChar(n);
        }

        /// <summary>
        /// Generates an upper-case alpha character
        /// </summary>
        /// <returns>A generator that generates an upper-case alpha character</returns>
        public static Gen<char> AlphaUpperChar()
        {
            return from n in Gen.choose('A', 'Z')
                   select Convert.ToChar(n);
        }

        /// <summary>
        /// Generates a lower-case alpha character
        /// </summary>
        /// <returns>A generator that generates a lower-case alpha character</returns>
        public static Gen<char> AlphaLowerChar()
        {
            return from n in Gen.choose('a', 'z')
                   select Convert.ToChar(n);
        }

        /// <summary>
        /// Generates an alpha character
        /// </summary>
        /// <returns>A generator that generates an alpha character</returns>
        public static Gen<char> AlphaChar()
        {
            return Any.WeighedGeneratorIn(
                new WeightAndValue<Gen<char>>(1, AlphaUpperChar()),
                new WeightAndValue<Gen<char>>(9, AlphaLowerChar()));
        }

        /// <summary>
        /// Generates an alphanumerical character
        /// </summary>
        /// <returns>A generator that generates an alphanumerical character</returns>
        public static Gen<char> AlphaNumChar()
        {
            return Any.WeighedGeneratorIn(
                new WeightAndValue<Gen<char>>(1, NumChar()),
                new WeightAndValue<Gen<char>>(9, AlphaChar()));
        }

        /// <summary>
        /// Generates a string of alpha characters
        /// </summary>
        /// <returns>A generator that generates a string of alpha characters</returns>
        public static Gen<string> AlphaStr()
        {
            return from cs in AlphaChar().MakeList()
                   let s = new string(cs.ToArray())
                   where s.All(Char.IsLetter)
                   select s;
        }

        /// <summary>
        /// Generates a string of digits
        /// </summary>
        /// <returns>A generator that generates a string of digits</returns>
        public static Gen<string> NumStr()
        {
            return from cs in NumChar().MakeList()
                   let s = new string(cs.ToArray())
                   where s.All(Char.IsDigit)
                   select s;
        }

        /// <summary>
        /// Generates a version 4 (random) UUID.
        /// </summary>
        /// <returns>A generator that generates a version 4 (random) UUID.</returns>
        public static Gen<Guid> Guid()
        {
            // http://en.wikipedia.org/wiki/Universally_unique_identifier#Version_4_.28random.29

            return from l1Upper in Gen.choose(0, int.MaxValue)
                   from l1Lower in Gen.choose(0, int.MaxValue)
                   from l2Upper in Gen.choose(0, int.MaxValue)
                   from l2Lower in Gen.choose(0, int.MaxValue)
                   let l1 = ((long) l1Upper << 32) + l1Lower
                   let l2 = ((long) l2Upper << 32) + l2Lower
                   from y in Gen.elements(new[] {'8', '9', 'a', 'b'})
                   select MakeGuidFromBits(l1, l2, y);
        }

        private static Guid MakeGuidFromBits(long l1, long l2, char y)
        {
            var s1 = l1.ToString("X16") + l2.ToString("X16");
            var chars = s1.ToCharArray();
            chars[12] = '4';
            chars[16] = y;
            var s2 = new string(chars);
            return System.Guid.Parse(s2);
        }
    }
}
