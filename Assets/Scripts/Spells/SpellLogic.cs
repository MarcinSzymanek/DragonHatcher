using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpellLogic : MonoBehaviour
{
	//public Spell spell;
	public UnityEvent onCast;
	public int id;
	public int castDelay;
	public string audioPath;
	
	private float castDelay_;
	private AudioClip[] audioClips_;
	
	
	private void Start(){
		castDelay_ = castDelay/1000f;
		audioClips_ = Resources.LoadAll<AudioClip>(audioPath);
	}
	
	public void CastSpell(){
		Invoke("cast_", castDelay_);
	}
	
	private void cast_(){
		onCast.Invoke();
	}
	
	public AudioClip GetAudioClip(){
		int index = Random.Range(0, audioClips_.Length);
		return audioClips_[index];
	}
}
