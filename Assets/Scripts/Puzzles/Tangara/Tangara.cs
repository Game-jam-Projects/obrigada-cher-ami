using UnityEngine;

public class Tangara : MonoBehaviour
{
    #region Fields

    private bool _isAlpha;
    private SpriteRenderer _spriteRenderer;

    #endregion

    #region Properties

    public bool IsAlpha => _isAlpha;

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

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _spriteRenderer.enabled = false;
    }

    #endregion

    #region Public Methods

    public void TriggerDance()
    {
        TangaraManager.Instance.StartDance();
    }

    public void ActivateSprite()
    {
        _spriteRenderer.enabled = true;
    }

    public void SetIsAlpha()
    {
        _isAlpha = true;

        //_spriteRenderer.color = Color.blue;
    }

    #endregion
}