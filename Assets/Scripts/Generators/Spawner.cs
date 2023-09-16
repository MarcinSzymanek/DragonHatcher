using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Spawner <T>
{
    void Spawn(GameObject T, int x, int y);
}
