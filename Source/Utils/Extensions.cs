using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arda
{
	internal static class Extensions
	{
		private static Random rng = new Random();

		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = rng.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static T GetRandom<T>(this IEnumerable<T> collection)
		{
			int element = rng.Next(0, collection.Count());
			return collection.ElementAt(element);
		}
	}
}
