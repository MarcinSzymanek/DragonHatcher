using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIStrategies;

public interface IAIBase
{
	public void OnTargetAcquired(object? sender, ObjectEnteredArgs args);
	public AIScan scanner{get;}
	public void SetStrategy(IAI_Strategy strategy);
}
