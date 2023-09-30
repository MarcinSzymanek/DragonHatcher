using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class ResourceManager : MonoBehaviour
{
	public static ResourceManager instance {get; private set;}
	private static Dictionary<ResourceID, string> res_path_dict;
	
	[SerializeReference]
	List<ResourceSO> res_list;
	bool loaded_ = false;
	public event System.Action<ResourceID, int> resource_updated;
	
	Dictionary<ResourceID, int> session_store_;
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
	        instance = this;
	        LoadData();
        }
    }
    
    
	// Load data from SO assets
	private void LoadData(){
		res_list = new List<ResourceSO>();
		session_store_ = new Dictionary<ResourceID, int>();
		res_path_dict = new Dictionary<ResourceID, string>(){
			{ResourceID.wood, "DataObjects/Wood"},
			{ResourceID.stone, "DataObjects/Stone"},
			{ResourceID.gold, "DataObjects/Gold"}
		};
		foreach(var item in res_path_dict){
			res_list.Add(Resources.Load<ResourceSO>(item.Value)); 
			session_store_[item.Key] = 0;
		}
		loaded_ = true;
	}
    
	// Reset resource count
	private void ResetResources(){
		if(!loaded_){
			LoadData();
		}	
		foreach(var item in session_store_){
			session_store_[item.Key] = 0;
		}
		
	}
	
	public string GetResIconpath(ResourceID id){
		return res_list.Find(x => x.ID == id).Icon;
	}
	
	public void Add(ResourceID id, int count){
		session_store_[id] += count;
		resource_updated?.Invoke(id, session_store_[id]);
	}
	
	public void Reduce(ResourceID id, int count){
		session_store_[id] += count;
		resource_updated?.Invoke(id, session_store_[id]);
	}
}
