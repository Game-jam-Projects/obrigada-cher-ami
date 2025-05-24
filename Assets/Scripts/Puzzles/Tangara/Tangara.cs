using UnityEngine;

public class Tangara : MonoBehaviour
{
    [SerializeField] private bool _isAlpha;

    public void SetIsAlpha()
    {
        _isAlpha = true;

        if (TryGetComponent(out SpriteRenderer renderer))
        {
            renderer.color = Color.blue;
        }
    }
}