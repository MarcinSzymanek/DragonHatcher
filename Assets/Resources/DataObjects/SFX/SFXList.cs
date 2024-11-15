using UnityEngine;

[CreateAssetMenu(fileName = "SFXList", menuName = "ScriptableObjects/SFXList")]
public class SFXList : ScriptableObject
{
	public AudioClip[] Clips;
}
