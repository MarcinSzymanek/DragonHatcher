using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base of the spell. T is parameter type of the spell method
// This class makes sure cast delay and cooldowns are being processed

public abstract class SpellBase<T> : MonoBehaviour, ISpell
{
	[field: SerializeField]
	public int id{get; set;}
	public bool onCooldown{get; private set;}
	[field: SerializeField]
	public float cooldown{get; protected set;}
	[field: SerializeField]
	public float castDelay{get; protected set;}
	[field: SerializeField]
	public SpellDataObject spellData{get; protected set;}
	abstract internal void onCast(T param);
	abstract internal T getTarget(Vector3 mousePos);
	
	[SerializeReference]
	private AudioClip[] sfx_;
	private ManagedSFX sfxBeforeEffect_;
	private AudioSource mainAudio_;
	
	void Start(){
		mainAudio_ = transform.root.GetComponentInChildren<AudioSource>();
		sfxBeforeEffect_ = GetComponentInChildren<ManagedSFX>();
	}
	
	void OnEnable(){
		onCooldown = false;	
	}
	
	public bool CastSpell(Vector3 mousePos){
		if(onCooldown) return false;
		T target = getTarget(mousePos);
		StartCoroutine(cast_(target));
		return true;
	}
	
	private IEnumerator cast_(T target){
		onCooldown = true;
		if(sfxBeforeEffect_ != null){
			sfxBeforeEffect_.PlaySfx();
		}
		yield return new WaitForSeconds(castDelay);
		if(sfx_.Length != 0){
			mainAudio_.PlayOneShot(sfx_[UnityEngine.Random.Range(0, sfx_.Length)]);
		}
		onCast(target);
		yield return new WaitForSeconds(cooldown);
		onCooldown = false;	
	}
}
