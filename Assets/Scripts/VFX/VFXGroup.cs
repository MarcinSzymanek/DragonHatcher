using UnityEngine;

public class VFXGroup : MonoBehaviour
{

	GameObject[] subObjects_;
	void Awake()
	{
		subObjects_ = new GameObject[transform.childCount];
		
		int i = 0;
		foreach(Transform child in transform)
		{
			subObjects_[i] = child.gameObject;
			i++;
		}
	}
	
	[ContextMenu("Turn on")]
	public void Activate()
	{
		foreach(GameObject obj in subObjects_)
		{
			obj.SetActive(true);
		}
	}
	
	public void Deactivate()
	{
		foreach(GameObject obj in subObjects_)
		{
			obj.SetActive(false);
		}
	}
	
	private void OnDisable()
	{
		Deactivate();
	}

}
