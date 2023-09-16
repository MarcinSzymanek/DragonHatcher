using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Helper construct for logic states
// We want to have explicit control over whether something is allowed or not
// Fx, when implementing stun effects, character actions should be locked so that they are not allowed to move/attack etc.
public class Locker<T>
    where T: MonoBehaviour
{
    bool lock_;

    public Locker(){
        lock_ = false;
    }

    public bool Locked {get => lock_;}
    
    // Explicit control over what is locked, what is not
    public void Lock(){
        lock_ = true;
    }

    public void Unlock(){
        lock_ = false;
    }
};
