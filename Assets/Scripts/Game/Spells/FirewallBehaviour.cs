using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirewallBehaviour : MonoBehaviour
{

    public float time;

    void Start()
    {
        Invoke("DestroyFirewall", time);
    }

    void DestroyFirewall()
    {
        Destroy(transform.parent.gameObject);
    }
}
