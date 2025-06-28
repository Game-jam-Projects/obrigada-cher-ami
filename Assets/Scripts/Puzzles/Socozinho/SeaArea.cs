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
        Gizmos.color = new Color(0f, 0.5f, 1f, 0.25f);

        Vector2 size = _maxBounds - _minBounds;
        Vector2 center = (_minBounds + _maxBounds) / 2;

        Gizmos.DrawCube(center, size);

        Gizmos.color = new Color(0f, 0.5f, 1f, 1f);
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