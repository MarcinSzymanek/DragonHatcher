using UnityEngine;
using System.Collections;

// This script will enable assigned gameobject after a delay
public class DelayedEnable : MonoBehaviour
{
	[Tooltip("Delay in seconds before objects are enabled")]	
	public float DelayInSeconds;
	[Tooltip("GameObjects to enable")]	
	public GameObject[] Objects;

    void Awake()
    {
	    StartCoroutine(enableAfter(DelayInSeconds));
    }
    
	private IEnumerator enableAfter(float delay)
	{
		yield return new WaitForSeconds(delay);
		foreach(GameObject o in Objects)
		{
			o.SetActive(true);
		}
		this.enabled = false;
	}

}
