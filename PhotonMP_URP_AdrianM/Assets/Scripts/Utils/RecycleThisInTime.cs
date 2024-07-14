using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecycleThisInTime : MonoBehaviour
{
    [SerializeField] private float _value;

    void OnEnable()
    {
        CancelInvoke(nameof(RecycleInTime));
        Invoke(nameof(RecycleInTime), _value);
    }

    private void RecycleInTime()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        gameObject.Recycle();
    }

    public void RecycleInTime(float arg_Value)
    {
        CancelInvoke(nameof(RecycleInTime));
        Invoke(nameof(RecycleInTime), arg_Value);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(RecycleInTime));
    }
}