using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// AIStrategies determine default targetting behaviour of AI enemies
namespace AIStrategies{
	
	public class StrategyTargetEgg : IAI_Strategy
	{
		public void Setup(IAIBase ai){
			var target = GameObject.FindGameObjectWithTag("Egg");
			ai.OnTargetAcquired(this, new ObjectEnteredArgs(target.transform));
		}
	}
	
	public class StrategyScanForPlayer : IAI_Strategy
	{
		public void Setup(IAIBase ai){
			ai.scanner.objectEntered += ai.OnTargetAcquired;
		}
	}
}
