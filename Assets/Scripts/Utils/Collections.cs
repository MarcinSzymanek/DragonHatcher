using UnityEngine;
using System.Collections.Generic;

namespace Utils
{
	public class Collections
	{
		public static T GetRandom<T>(IList<T> collection)
		{
			int it = Random.RandomRange(0, collection.Count);
			return collection[it];
		}
		
	}
}
