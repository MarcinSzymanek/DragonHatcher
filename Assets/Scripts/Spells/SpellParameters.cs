using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellParameters
{
	public SpellParameters(VectorTarget vtarget = null){
		vectorTarget = vtarget;
	}
	
	public VectorTarget? vectorTarget{get; private set;}

}
