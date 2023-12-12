using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Tornado : MonoBehaviour, ISpell
{
    public GameObject tornadoPrefab;
    public Transform firePoint;

    public LayerMask targetLayer;
    public LayerMask ownerLayer;

    // How many times must we bitshift to set the layer: integer value of the layer
    private int layerInt_ = 0;

    public int id
    {
        get { return id_; }
        set { }
    }
    public int id_ = 2;
    string name_;
    public string name { get => name_; }

    public float speed = 10f;

    void Start()
    {
        int layerVal = ownerLayer.value;
        while (layerVal > 1)
        {
            layerVal /= 2;
            layerInt_++;
        }
        // Debug.Log("Layer int is: " + layerInt_.ToString());
    }

    public void CastSpell(SpellParameters parameters)
    {
        SpawnTornado(parameters.vectorTarget);
    }

    public void CastSpell(Vector3 mousePos)
    {
        Vector2 direction = ((Vector2)mousePos - (Vector2)firePoint.position).normalized;
        
        VectorTarget target = new VectorTarget(firePoint.position, direction);
        SpawnTornado(target);
    }

    private void SpawnTornado(VectorTarget target)
    {
        GameObject tornado = Instantiate(tornadoPrefab, firePoint.position, firePoint.rotation);
        setPrefabTarget(tornado);
        Rigidbody2D rb = tornado.GetComponent<Rigidbody2D>();
        rb.velocity = target.direction * speed;

        tornado.transform.rotation = Quaternion.Euler(new Vector3(0, 0, target.angle));
    }

    void setPrefabTarget(GameObject obj)
    {
        obj.layer = layerInt_;
        var col = obj.transform.GetChild(1).GetComponent<Collider2D>();
        col.gameObject.layer = layerInt_;

        col = obj.transform.GetChild(0).GetComponent<Collider2D>();
        col.callbackLayers += targetLayer;
        col.contactCaptureLayers += targetLayer;
        col.gameObject.layer = layerInt_;
        var rb = obj.GetComponent<Rigidbody2D>();
    }
}