﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
	
	private void OnTriggerStay2D(Collider2D collision)
	{   
        // Say what it should trigger on
		Destroy(transform.parent.gameObject);
    }
}