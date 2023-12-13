using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellCooldownIndicator : MonoBehaviour
{
	private float maxPaddingRight_ = 75;
	private RectMask2D rectMask_;
	private Image image_;
	private float baseAlpha_;
	
	private void Awake(){
		rectMask_ = GetComponent<RectMask2D>();
		image_ = GetComponentInChildren<Image>();
		baseAlpha_ = 160/255f;
	}
	
	public void StartCooldownIndicator(float cooldown){
		Debug.Log("Start cooldown ind, cd= " + cooldown.ToString());
		image_.color = new Color(image_.color.r, image_.color.g, image_.color.b, baseAlpha_);
		StartCoroutine(ProgressCooldown(cooldown));
	}
	
	IEnumerator ProgressCooldown(float cooldown){
		int add_to_mask = 1;
		float step = cooldown/maxPaddingRight_;
		while (step < 0.01f){
			step = step * 2;
			add_to_mask = add_to_mask * 2;
		}
		Debug.Log("Single step: " + step.ToString());
		
		while(rectMask_.padding.z < maxPaddingRight_){
			rectMask_.padding = new Vector4(0, 0, rectMask_.padding.z + add_to_mask, 0);
			yield return new WaitForSeconds(step);
		}
		image_.color = new Color(image_.color.r, image_.color.g, image_.color.b, 0);
		rectMask_.padding = new Vector4(0, 0, 0, 0);
	}
}
