using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[CreateAssetMenu(fileName="Enemy", menuName = "ScriptableObjects/Enemy")]
[Serializable]
public class Enemy : ScriptableObject{
	public int Health;
	public int Damage;
	public Sprite BaseSprite;
}