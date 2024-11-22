using UnityEngine;

public class ConeDamageSpell : SpellBase, IParticleSubscriber
{
	[Tooltip("How far from the player the spell effect starts at")]
	public float DistanceFromPlayer = 0.75f;
	public Transform rootObj;

	private ParticleSystem particleSystem_;
	
	private void Awake()
	{
		particleSystem_ = GetComponentInChildren<ParticleSystem>();
	}
	
	internal override void onCast()
	{
		Vector3 mousePos = Math2d.GetMousePos();
		Vector3 direction = Math2d.CalcDirection(rootObj.position, mousePos);
		//MathVisualise.DrawArrow(rootObj, direction, 2f);
		Vector3 newPosition = direction * DistanceFromPlayer;
		transform.localPosition = newPosition;
		float rotation = Math2d.GetDegreeFromVector(newPosition);
		transform.localEulerAngles = new Vector3(0, 0, rotation);
		particleSystem_.Play();
		GetComponentInChildren<ParticleStoppedNotifier>().SetSubscriber(this);
	}
	
	public void OnParticleEnd()
	{
	}
}
