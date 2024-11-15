using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
	public enum ObjectType
	{
		DUST_VFX
	}

	[Tooltip("These objects will be added to disabledObjects pool in Awake. Enabled objects will add themselves in OnEnable instead")]
	public PoolableObject[] StartingObjects;
	private List<PoolableObject> activeObjects_;
	private List<PoolableObject> disabledObjects_;

    void Awake()
	{
		activeObjects_ = new List<PoolableObject>();
		disabledObjects_ = new List<PoolableObject>();
		
		foreach(PoolableObject obj in StartingObjects)
		{
			disabledObjects_.Add(obj);
			obj.RegisterPooler(this);
		}
	}
    
	// Return a disabled object, if available. Otherwise return null
	public GameObject? GetFreeObject()
	{
		if(disabledObjects_.Count > 0)
		{
			return disabledObjects_[disabledObjects_.Count - 1].gameObject;
		}
		else
		{
			Debug.LogWarning("Pool could not deliver object");
			return null;
		}
	}
	
	public void SetActive(PoolableObject poolable)
	{
		if(disabledObjects_.Contains(poolable))
		{
			disabledObjects_.Remove(poolable);
			activeObjects_.Add(poolable);
		}
	}
	
	public void SetDisabled(PoolableObject poolable)
	{
		if(activeObjects_.Contains(poolable))
		{
			activeObjects_.Remove(poolable);
			disabledObjects_.Add(poolable);
		}
	}
	
	public void Remove(PoolableObject poolable)
	{
		if(activeObjects_.Contains(poolable))
		{
			activeObjects_.Remove(poolable);
		}
		else if(disabledObjects_.Contains(poolable))
		{
			disabledObjects_.Remove(poolable);
		}
	}
}
