using UnityEngine;

public class SeaArea : Singleton<SeaArea>
{
    #region Fields

    [SerializeField] private Vector2 _minBounds;
    [SerializeField] private Vector2 _maxBounds;

    public Vector2 MinBounds => _minBounds;
    public Vector2 MaxBounds => _maxBounds;

    #endregion

    #region Unity Methods

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Vector2 size = _maxBounds - _minBounds;
        Vector2 center = (_minBounds + _maxBounds) / 2;

        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(center, size);
    }

#endif

    #endregion

    #region Public Methods

    public bool IsInsideSea(Vector2 position)
    {
        return position.x >= MinBounds.x && position.x <= MaxBounds.x &&
               position.y >= MinBounds.y && position.y <= MaxBounds.y;
    }

    #endregion
}