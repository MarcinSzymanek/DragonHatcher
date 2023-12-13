using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class ResourceManager : MonoBehaviour
{
	public static ResourceManager instance {get; private set;}
	private static Dictionary<ResourceID, string> res_path_dict;
	private AudioSource audios_;
	
	private float[] pitch_vals = {1f, 0.9f, 0.8f, 0.7f, 0.5f};
	
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
	        audios_ = GetComponent<AudioSource>();
	        LoadData();
        }
    }
    
    
	// Load data from SO assets
	private void LoadData(){
		res_list = new List<ResourceSO>();
		session_store_ = new Dictionary<ResourceID, int>();
		res_path_dict = new Dictionary<ResourceID, string>(){
		};
		
		foreach (var asset in Resources.LoadAll<ResourceSO>("DataObjects/")){
			res_list.Add(asset);
			session_store_[asset.ID] = 0;
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
	
	public bool CheckPlayerHasResources(List<ResourceCost> cost){
		foreach (ResourceCost item in cost)
		{
			if(session_store_[item.id] < item.amount) return false;
		}
		return true;
	}
	
	public bool ProcessTransaction(List<ResourceCost> cost){
		Debug.Log("Processing transaction...");
		var old_session_state = session_store_;
		bool success = true;
		foreach (ResourceCost item in cost)
		{
			Debug.Log("item: " + item.id.ToString());
			if(session_store_[item.id] < item.amount) success = false;
			Debug.Log("Updating " + item.id.ToString() + " -" + item.amount);
			session_store_[item.id] -= item.amount;
			resource_updated?.Invoke(item.id, session_store_[item.id]);
		}
		if(!success) session_store_ = old_session_state;
		return success;
	}
	
	public void Add(ResourceID id, int count){
		Debug.Log("Adding " + count.ToString() + " of " + id.ToString());
		session_store_[id] += count;
		Debug.Log("New val: " + session_store_[id]);
		resource_updated?.Invoke(id, session_store_[id]);
		audios_.PlayOneShot(audios_.clip);
		audios_.pitch = pitch_vals[Random.Range(0, 4)];
	}
	
	public void Reduce(ResourceID id, int count){
		session_store_[id] += count;
		resource_updated?.Invoke(id, session_store_[id]);
	}
}
