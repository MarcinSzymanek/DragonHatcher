using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;


 
// Store spells using ScriptableObject
// Save game later?
public class SessionDataStore : MonoBehaviour
{

	const string cacheKeyName = "sessionData";
	static SessionData sessionData_;
	void Awake(){
		if(sessionData_ = null){
			sessionData_ = ScriptableObject.CreateInstance<SessionData>();
			sessionData_.cachedSpells = new List<SpellDataObject>();
		}
	}
	
	public void SaveSpells(){
		string spellCacheJSON = JsonUtility.ToJson(sessionData_);
		PlayerPrefs.SetString(cacheKeyName, spellCacheJSON);
	}
	
	public void LoadSpells(){
		if(!PlayerPrefs.HasKey(cacheKeyName)) return;
		
		string spellCacheJSON = PlayerPrefs.GetString(cacheKeyName);
		sessionData_ = JsonUtility.FromJson<SessionData>(spellCacheJSON);
	}
}
