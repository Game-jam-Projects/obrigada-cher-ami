using UnityEngine;
using UnityEngine.InputSystem;

public class OlhaMouse : MonoBehaviour
{
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
        transform.up = direction;
    }
}
