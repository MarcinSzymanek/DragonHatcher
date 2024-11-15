using UnityEngine;

// Helper methods to change state in 2 or more animators at once
public class CompositeAnimation : MonoBehaviour
{
	public Animator[] Animators;

	public void SetMoving(bool val)
	{
		foreach (var anim in Animators)
		{
			anim.SetBool("IsMoving", val);
		}
	}
	
	public void SetFacing(bool left){
		foreach (var anim in Animators)
		{
			anim.SetBool("FaceLeft", left);
		}
	}

}
