using UnityEngine;
using UnityEngine.InputSystem;

public class OlhaMouse : MonoBehaviour
{
    private Rigidbody2D Rigidbody;

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        var lookDir = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        Rigidbody.SetRotation(angle);
    }
}
