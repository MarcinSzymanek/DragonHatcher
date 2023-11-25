using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellAnimEvents : MonoBehaviour
{

    public void onAnimFinished() {
        Destroy(gameObject);
    }
}
