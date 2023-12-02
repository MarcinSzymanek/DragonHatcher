using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Figure out what to do when AI enters the scene
namespace AIStrategies{
	public interface IAI_Strategy
	{
		void Setup(IAIBase ai);
	}	
}

