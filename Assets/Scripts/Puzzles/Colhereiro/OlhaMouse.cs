using UnityEngine;
using UnityEngine.InputSystem;

public class OlhaMouse : MonoBehaviour
{
    private bool _canMove = false;
    void Update()
    {
        if (_canMove)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 direction = (mousePos - (Vector2)transform.position).normalized;
            transform.up = direction;
        }
    }

    public void BloquearMovimento() => _canMove = false;
    public void DesbloquearMovimento() => _canMove = true;
}
