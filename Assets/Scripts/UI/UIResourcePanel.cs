using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIResourcePanel : MonoBehaviour
{
	Dictionary<ResourceID, UIResource> component_handles;
	void Start()
    {
	    ResourceManager.instance.resource_updated += OnUpdate;
	    component_handles = new Dictionary<ResourceID, UIResource>();
	    var comps = GetComponentsInChildren<UIResource>();
	    foreach(var item in comps){
	    	component_handles[item.ID] = item;
	    }
    }
    
	public void OnUpdate(ResourceID id, int count){
		component_handles[id].UpdateText(count.ToString());
	}

}
