using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimEvents : MonoBehaviour
{
	Animator anim_;
    // Start is called before the first frame update
    void Start()
    {
	    anim_ = GetComponent<Animator>();
    }

	public void OnIdleLoop(){
		anim_.SetFloat("idleIter", anim_.GetFloat("idleIter") + 1);
	}
	
	public void OnIdleExit(){
		anim_.SetFloat("idleIter", 0);
	}
}
