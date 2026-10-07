using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimationEventIntTool : MonoBehaviour
{
    // unity variables
    public int parameter;
    public UnityEvent<int> useInt;

    public void TriggerIntEvent()
    {
        useInt.Invoke(parameter); // safe to invoke even without callbacks
    }
}