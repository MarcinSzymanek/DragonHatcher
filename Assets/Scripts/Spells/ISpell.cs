using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISpell
{
	// void CastSpell(SpellParameters parameters);
	public int id{get; set;}
	public string name{get;}
	void CastSpell(Vector3 mousePos);
}

internal interface ISpellEffect<T>
{
	void OnCast(T param);
}