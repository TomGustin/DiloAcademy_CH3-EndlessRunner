using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderBound : MonoBehaviour
{
    [SerializeField] private UnityEvent OnColliderAction;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        OnColliderAction?.Invoke();
    }
}
