using UnityEngine;
using UnityEngine.Events;

public class OnClickDoAction : MonoBehaviour
{
    public UnityEvent Response;

    void OnMouseDown() => Response.Invoke();
}
