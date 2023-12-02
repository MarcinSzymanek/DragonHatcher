using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellParameters
{
	public SpellParameters(VectorTarget vtarget = null){
		vectorTarget = vtarget;
	}
	
	public VectorTarget? vectorTarget{get; private set;}

	public SpellParameters(PointTarget ptarget = null)
	{
		pointTarget = ptarget;
	}

	public PointTarget? pointTarget { get; private set;}

}