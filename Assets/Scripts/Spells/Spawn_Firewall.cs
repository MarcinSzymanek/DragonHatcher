using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Firewall : MonoBehaviour, ISpell
{
    public GameObject firewallPrefab;

    public int id
    {
        get { return id_; }
        set { }
    }
    public int id_ = 1;
    string name_;
    public string name { get => name_; }

    public void CastSpell(SpellParameters parameters)
    {
        SpawnFirewall(parameters.pointTarget);
    }

    private void SpawnFirewall(PointTarget pt)
    {
        Vector3 position = new Vector3(pt.x, pt.y, 0);
        Instantiate(firewallPrefab, position, Quaternion.identity);
    }
}
