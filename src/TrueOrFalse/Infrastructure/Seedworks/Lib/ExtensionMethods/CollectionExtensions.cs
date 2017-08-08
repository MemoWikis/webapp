using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seedworks.Lib
{
	public static class CollectionExtensions
	{
		/// <summary>
		/// Shuffles the items in the list, changes the list! Uses a Random instance with changing seed.
		/// </summary>
		/// <remarks>
		/// User Fisher-Yates (see http://en.wikipedia.org/wiki/Fisher-Yates_shuffle)
		/// recommended here: http://nunos.zi-yu.com/2008/03/shuffle-a-collection/
		/// </remarks>
		public static IList Shuffle(this IList elements)
		{
			var random = new Random();

			int n = elements.Count;

			while (n > 1)
			{
				int k = random.Next(n);
				--n;
				var temp = elements[n];
				elements[n] = elements[k];
				elements[k] = temp;
			}

			return elements;
		}

		/// <summary>
		/// Shuffles the items in the list, changes the list! 
		/// Uses a <see cref="Random"/> instance with changing seed.
		/// <br/>
		/// Strongly typed version of <see cref="Shuffle"/>.
		/// </summary>
		/// <typeparam name="TList"></typeparam>
		/// <param name="list"></param>
		/// <returns></returns>
		public static TList Shuffle<TList>(this TList list) where TList : IList
		{
			var random = new Random();
			
			int n = list.Count;

			while (n > 1)
			{
				int k = random.Next(n);
				--n;
				var temp = list[n];
				list[n] = list[k];
				list[k] = temp;
			}

			return list;
		}

		/// <summary>
		/// Returns all elements of a collection that occur more than once in the given collection.
		/// </summary>
		public static IEnumerable<T> Duplicates<T>(this IEnumerable<T> collection)
		{
			return collection.Where(item1 => collection.Count(item2 => item2.Equals(item1)) > 1);
		}

		/// <summary>
		/// Does the same as Where() but does not throw an exception when the collection is empty.
		/// </summary>
		public static IEnumerable<TItem> WhereSafe<TItem>(this IEnumerable<TItem> coll, Func<TItem, bool> predicate)
		{
			if (coll.Count() <= 0)
				return coll;

			return coll.Where(predicate);
		}


		/// <summary>
		/// Converts an IEnumeable to a generic list based on the given type.
		/// @deprecated
		/// </summary>
        [Obsolete("Why is this solved in this way? Isn't coll.Cast<T>().ToList() enough?")]
		public static IList AsListOf(this IEnumerable coll, Type type)
		{
            var result = (IList) typeof (List<>).MakeGenericType(type).GetConstructor(Type.EmptyTypes).Invoke(null);
			foreach (var item in coll) result.Add(item);
			return result;
		}

		public static List<T> Add<T>(this List<T> list, params T[] items)
		{
			list.AddRange(items);
			return list;
		}

        public static IList<T> AsListOf<T>(this IEnumerable coll)
        {
            return coll.Cast<T>().ToList();
        }
	}
}
