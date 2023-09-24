using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ResourceFactory : MonoBehaviour
{
    public static ResourceFactory instance {get; private set;}
    // private Dictionary<ResourceID, ResourceObj> res_dict = new Dictionary<ResourceID, ResourceObj>();

    private void fillDict(){
        
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            
        }
    }

    // public static ResourceObj GetResource(ResourceID id){
    //    throw 0;
    //}
}
