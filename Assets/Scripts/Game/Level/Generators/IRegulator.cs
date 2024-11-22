using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRegulator
{
    float Threshold { get; set; }
    void Regulate();
}
