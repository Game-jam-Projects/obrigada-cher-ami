using UnityEngine;
using UnityEngine.InputSystem;

public class Bicada : MonoBehaviour
{
    [SerializeField] private Vector3 EsticaPos;
    [SerializeField] private GameEvent BicadaEvent;
    private readonly float MoveDuration = 0.2f;
    private readonly float MaxVelocity = 2000f;

    private InputAction _attackAction;
    private Vector2 _currentVelocity;
    private Vector3 _initialPos;

    void Start()
    {
        _initialPos = transform.localPosition;
        _attackAction = InputSystem.actions.FindAction("Attack");
    }

    void Update()
    {
        if (transform.localPosition != _initialPos)
            ResetaPosicao();

        if (_attackAction.triggered)
        {
            transform.localPosition = EsticaPos;
            BicadaEvent.Broadcast();
        }
    }

    private void ResetaPosicao()
    {
        transform.localPosition = Vector2.SmoothDamp(transform.localPosition, _initialPos, ref _currentVelocity, MoveDuration - 0.1f, MaxVelocity);
    }
}
