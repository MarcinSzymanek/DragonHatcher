using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils{
	
	public class StringUtils<T>
	{
		public static void PrintList(List<T> list){
			foreach(T item in list){
				Debug.Log(item.ToString());
			}
		}
	
		public static void PrintList(List<MonoBehaviour> list){
			foreach(MonoBehaviour item in list){
				if(item.transform.parent != null){
					Debug.Log(item.transform.parent.gameObject.name + ": " + item.name);
					continue;
				}
				Debug.Log(item.name);
			}
		}
	}
	
}
