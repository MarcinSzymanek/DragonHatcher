﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneProperties : MonoBehaviour
{
	public enum SceneType{
		WAVE_DEFENCE,
		DUNGEON_CRAWL
	};
	
	public SceneType sceneType;
}