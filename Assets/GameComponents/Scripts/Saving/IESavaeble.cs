using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IESavaeble
{
    object CaptureState();
    void RestoreState(object state);
}