using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Items
{
    public class PotionScript : MonoBehaviour
    {
        private int currentHealth;
        private Health healthScript;
        private TakeDamage takeDamageScript;
        private void Awake()
        {
            GameObject player = GameObject.Find("Player");
            //healthScript = player.GetComponent<Health>();
            takeDamageScript = player.GetComponent<TakeDamage>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision != null && currentHealth != 50)
            {
                //healthScript.currentHealth += 10;
                takeDamageScript.TriggerTakeDamage(-10);
                Destroy(gameObject);
            }
        }
    }
}