using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider2D))]
public class DragonEggInteractable : MonoBehaviour, IInteractable
{
	TextMeshPro text_;
	private bool triggered = false;
	
	void Awake()
    {
	    text_ = GetComponentInChildren<TextMeshPro>();
    }
    
	public void OnInteract()
	{
		if(triggered) return;
		Debug.Log("Egg triggered");
		GameController.Instance.TriggerEnemySpawn();
		triggered = true;
	}
	
	public Vector3 GetPosition()
	{
		return transform.parent.position;
	}

	public void ToggleInteractableText()
	{
		if(text_.enabled)
		{
			text_.enabled = false;
			return;
		}
		text_.enabled = true;
	}
}
