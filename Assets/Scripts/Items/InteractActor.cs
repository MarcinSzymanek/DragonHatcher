using UnityEngine;
using System.Collections.Generic;

// This character can interact with interactable features
public class InteractActor : MonoBehaviour
{
	private List<IInteractable> interactablesInRange;
	public Transform RootObjectTf;
	private int interactableSize = 0;

    void Awake()
    {
	    interactablesInRange = new List<IInteractable>();
    }
	
	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
		{
			if(interactablesInRange.Contains(interactable)) return;
			interactablesInRange.Add(interactable);
			interactable.ToggleInteractableText();
			interactableSize++;
		}
	}
	
	private void OnTriggerExit2D(Collider2D other)
	{
		if(other.transform.TryGetComponent<IInteractable>(out IInteractable interactable))
		{
			if(interactablesInRange.Contains(interactable))
			{
				interactablesInRange.Remove(interactable);
				interactable.ToggleInteractableText();	
				interactableSize--;	
			}
		}
	}
	
	public void Interact()
	{
		IInteractable? chosenInteractable = null;
		float shortestDistance = float.MaxValue;

		// Go through the list of interactables
		// Interact with the one closest to the actor
		foreach(IInteractable feature in interactablesInRange)
		{
			if(chosenInteractable is null)
			{
				chosenInteractable = feature;
				shortestDistance = Math2d.CalcDistance(RootObjectTf.position, feature.GetPosition());
				continue;
			}
			float distance = Math2d.CalcDistance(RootObjectTf.position, feature.GetPosition());
			if(distance < shortestDistance)
			{
				chosenInteractable = feature;
			}
		}
		if(chosenInteractable is not null)
		{
			chosenInteractable.OnInteract();
		}
	}


}
