using UnityEngine;
using System.Collections;

public class Despawn : MonoBehaviour
{
	private SpriteRenderer spriteRenderer_;
	private Animator anim_;
	public GameObject[] DisableOnDespawn;
	public float DisableObjectAfter = -1f;
	public bool DestroyAfter = false;

	void Awake()
	{
		spriteRenderer_ = GetComponent<SpriteRenderer>();
	    anim_ = GetComponent<Animator>();
    }
    
	public void Trigger()
	{
		GetComponent<LinearAcceleration>().SetSpeed(1);
		anim_.SetTrigger("Despawn");
	}
	
	// Disable provided GameObjects. Use as a callback from animation to make sure everything is in sync
	public void DisableGameObjects()
	{
		foreach(GameObject obj in DisableOnDespawn)
		{
			obj.SetActive(false);
		}
		spriteRenderer_.enabled = false;
		anim_.enabled = false;
		if(DisableObjectAfter > 0)
		{
			StartCoroutine(DelayedDisable(DisableObjectAfter));
		}
	}
	
	private IEnumerator DelayedDisable(float delay)
	{
		yield return new WaitForSeconds(delay);
		
		if(DestroyAfter)
		{
			Destroy(gameObject);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}

}
