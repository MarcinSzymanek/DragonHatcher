using UnityEngine;
using UnityEngine.Events;

public interface IInteractable
{
	public void OnInteract();
	public Vector3 GetPosition();
	public void ToggleInteractableText();
}
