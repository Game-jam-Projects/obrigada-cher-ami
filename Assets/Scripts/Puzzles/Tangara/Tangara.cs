using UnityEngine;

public class Tangara : MonoBehaviour
{
    #region Fields

    private bool _isAlpha;

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

    #endregion

    #region Public Methods

    public void TriggerDance()
    {
        TangaraManager.Instance.StartDance();
    }

    public void SetIsAlpha()
    {
        _isAlpha = true;

        GetComponentInChildren<SpriteRenderer>().color = Color.blue;
    }

    #endregion
}