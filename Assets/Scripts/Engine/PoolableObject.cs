using UnityEngine;

public class PoolableObject : MonoBehaviour
{
	private ObjectPooler pooler_;

	private void OnEnable()
	{
		pooler_.SetActive(this);
	}
	
	private void OnDisable()
	{
		pooler_.SetDisabled(this);
	}
	
	public void RegisterPooler(ObjectPooler pooler)
	{
		pooler_ = pooler;
	}
}
