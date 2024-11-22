using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class of a spell with no cast parameters
// This class makes sure cast delay and cooldowns are being processed
public abstract class SpellBase : MonoBehaviour, ISpell
{
	[Header("Spell data")]
	// void CastSpell(SpellParameters parameters);
	[field: SerializeField]
	public int id{get; set;}
	public bool onCooldown{get; private set;}
	[field: SerializeField]
	new public string name{get; private set;}
	[field: SerializeField]
	public float cooldown{get; protected set;}
	[field: SerializeField]
	public float castDelay{get; protected set;}

	[field: SerializeField]
	public SpellDataObject spellData{get; protected set;}
	
	private void readSpellData()
	{
		name = spellData.name;
		cooldown = spellData.cooldown;
	}
	
	// This is to be implemented by particular spells
	abstract internal void onCast();
	
	[Header("Spell SFX")]
	[SerializeReference]
	private AudioClip[] sfx_;
	private ManagedSFX sfxBeforeEffect_;
	private AudioSource mainAudio_;
	
	void Awake()
	{
		Debug.Log("Spellbase Awake");
	}
	
	void Start(){
		readSpellData();
		mainAudio_ = transform.root.GetComponentInChildren<AudioSource>();
		sfxBeforeEffect_ = GetComponentInChildren<ManagedSFX>();
	}
	
	void OnEnable(){
		onCooldown = false;	
	}
	
	public bool CastSpell(){
		if(onCooldown) return false;
		StartCoroutine(cast_());
		return true;
	}
	
	private IEnumerator cast_(){
		onCooldown = true;
		if(sfxBeforeEffect_ != null){
			sfxBeforeEffect_.PlaySfx();
		}
		yield return new WaitForSeconds(castDelay);
		if(sfx_.Length != 0){
			mainAudio_.PlayOneShot(sfx_[UnityEngine.Random.Range(0, sfx_.Length)]);
		}
		onCast();
		yield return new WaitForSeconds(cooldown);
		onCooldown = false;	
	}
}

// Base class of a spell. T is target parameter type of the spell method
// This class makes sure cast delay and cooldowns are being processed
public abstract class SpellBase<T> : MonoBehaviour, ISpell<T>
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
	abstract internal T getTarget();
	
	[SerializeReference]
	private AudioClip[] sfx_;
	private ManagedSFX sfxBeforeEffect_;
	private AudioSource mainAudio_;
	
	void Start(){
		Debug.Log("SpellBase<T> start");
		mainAudio_ = transform.root.GetComponentInChildren<AudioSource>();
		sfxBeforeEffect_ = GetComponentInChildren<ManagedSFX>();
	}
	
	void OnEnable(){
		onCooldown = false;	
	}
	
	public bool CastSpell(T param){
		if(onCooldown) return false;
		T target = getTarget();
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
