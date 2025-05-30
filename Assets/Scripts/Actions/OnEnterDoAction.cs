using System;
using UnityEngine;
using UnityEngine.Events;

public class OnEnterDoAction : MonoBehaviour
{
    public UnityEvent Action;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Action.Invoke();
        }
    }
}
