using UnityEngine;
using UnityEngine.Events;

public class OnEnableDoAction : MonoBehaviour
{
    public UnityEvent Action;
    private void OnEnable()
    {
        Action.Invoke();
    }
}
