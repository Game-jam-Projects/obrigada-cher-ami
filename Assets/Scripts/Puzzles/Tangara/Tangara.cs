using UnityEngine;

public class Tangara : MonoBehaviour
{
    #region Fields

    [SerializeField] private bool _isAlpha;

    #endregion

    #region Unity Methods

    private void OnMouseDown()
    {
        if (_isAlpha)
        {
            TangaraManager.Instance.PlayerWins();
        }
        else
        {
            TangaraManager.Instance.PlayerLoses();
        }
    }

    #endregion

    #region Public Methods

    public void SetIsAlpha()
    {
        _isAlpha = true;

        if (TryGetComponent(out SpriteRenderer renderer))
        {
            renderer.color = Color.blue;
        }
    }

    #endregion
}