using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISpell
{
	int id{get;}
	string name{get;}
	void CastSpell(SpellParameters parameters);
	void CastSpell(Vector3 mousePos);
}
