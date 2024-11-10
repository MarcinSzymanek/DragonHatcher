using UnityEngine;

public class DragonEggInteractable : MonoBehaviour, IInteractable
{

	private bool triggered = false;

	void Awake()
    {
        
    }
    
	public void OnInteract()
	{
		if(triggered) return;
		Debug.Log("Egg triggered");
		//GameController.Instance.TriggerSpawn();	
		triggered = true;	
	}

}
