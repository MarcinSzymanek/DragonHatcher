using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProperties : MonoBehaviour
{
	public enum SceneType{
		LOADING,
		WAVE_DEFENCE,
		DUNGEON_CRAWL
	};
	
	public SceneType sceneType;
	
	public int maxEnemies;
	public int difficulty = 1;
}
