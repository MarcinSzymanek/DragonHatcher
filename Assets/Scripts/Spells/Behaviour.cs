using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {   
        // Say what it should trigger on
        Destroy(gameObject);
    }
}
