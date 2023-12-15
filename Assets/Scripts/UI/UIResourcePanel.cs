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
    	// Update all resource values
    	var resources = ResourceManager.instance.GetPlayerResources();
    	foreach (KeyValuePair<ResourceID, int> res in resources)
    	{
    		OnUpdate(res.Key, res.Value);
    	}
    }
    
	public void OnUpdate(ResourceID id, int count){
		if(!component_handles.ContainsKey(id)) return;
		component_handles[id].UpdateText(count.ToString());
	}
	
	// Clean up
	protected void OnDestroy()
	{
		ResourceManager.instance.resource_updated -= OnUpdate;
	}

}
