using UnityEngine;

public class AnimatorEventTrigger : MonoBehaviour
{
    //Trigado pelo Animator do objeto
    public void Broadcast(GameEvent gameEvent) => gameEvent.Broadcast();
}
