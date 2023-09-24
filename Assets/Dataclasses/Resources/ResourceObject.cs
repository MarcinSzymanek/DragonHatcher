using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResourceID{
    wood,
    stone,
    gold
}
[CreateAssetMenu(fileName="Resource", menuName = "ScriptableObjects/Resource")]
public class ResourceObject : ScriptableObject
{
    public string Name;
    public string Icon;
    public string SpritePickable;
    public ResourceID ID;
    public int Tier;

}
