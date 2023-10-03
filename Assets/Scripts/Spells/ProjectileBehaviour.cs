using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.InputSystem;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;
using System.Numerics;
using Unity.VisualScripting;

public class ProjectileBehaviour : MonoBehaviour
{
    public GameObject arrowPrefab;
    public float arrowSpeed = 3f;
    public int damage = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            TakeDamage damageComponent = collision.gameObject.GetComponent<TakeDamage>();
            if (damageComponent != null)
            {
                damageComponent.TakeDamageOnHit(damage);
            }

            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShootArrow()
    {
        if(gameObject != null && !gameObject.CompareTag("Arrow"))
        {
            GameObject newArrow = Instantiate(arrowPrefab, transform.position, transform.rotation);

            Rigidbody2D rb = newArrow.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = transform.right * arrowSpeed;
            }
            else
            {
                Destroy(newArrow);
            }
        }
    }
}