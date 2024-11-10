using System.Collections;
using UnityEngine;

namespace Utils
{
	public class Enumerators
	{
		// Do action until condition is satisfied
		public static IEnumerator DoUntil(System.Action action, System.Func<bool> condition){
			while(!condition()){
				action();
				yield return new WaitForSeconds(0.1f);
			}
		}
		
		// Do action until condition is satisfied. Wait tickInSeconds between each iteration
		public static IEnumerator DoUntil(System.Action action, System.Func<bool> condition, float tickInSeconds){
			while(!condition()){
				action();
				yield return new WaitForSeconds(tickInSeconds);
			}
		}
		
		// Do loopAction until condition is satisfied. Perform onFinish after the condition is satisfied
		public static IEnumerator DoUntilAndThen(System.Action loopAction, System.Func<bool> condition, System.Action onFinish){
			while(!condition()){
				loopAction();
				yield return new WaitForSeconds(0.1f);
			}
			onFinish();
		}
		
		// Do loopAction until condition is satisfied. Wait tickInSeconds between each iteration. Perform onFinish after the condition is satisfied
		public static IEnumerator DoUntilAndThen(System.Action loopAction, System.Func<bool> condition, System.Action onFinish, float tickInSeconds){
			while(!condition()){
				loopAction();
				yield return new WaitForSeconds(tickInSeconds);
			}
			onFinish();
		}
		
		// Call action after seconds
		public static IEnumerator DoAfter(System.Action action, float seconds){
			yield return new WaitForSeconds(seconds);
			action();
		}
	}
}