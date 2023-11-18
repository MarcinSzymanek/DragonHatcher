using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpell
{
	int id{get;}
	string name{get;}
	void CastSpell(SpellParameters parameters);
}
