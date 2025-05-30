using System;
using Unity.Cinemachine;
using UnityEngine;

public class AdjustCinemachineOrtho : MonoBehaviour
{
    public CinemachineCamera VirtualCamera;
    public float NewOrthoSize = 8;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            VirtualCamera.Lens.OrthographicSize = NewOrthoSize;
        }
    }
}
