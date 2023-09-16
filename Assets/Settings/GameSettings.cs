using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
	bool autoEquip_ = true;
	bool AutoEquip{get => autoEquip_;}
	public static GameSettings Instance;
    // Start is called before the first frame update
	void Awake()
	{
		if(Instance == null) Instance = this;
	    DontDestroyOnLoad(gameObject);
    }
}
