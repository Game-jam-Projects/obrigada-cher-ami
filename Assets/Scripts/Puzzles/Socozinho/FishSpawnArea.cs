using UnityEngine;

public class FishSpawnArea : Singleton<FishSpawnArea>
{
    #region Fields

    private Vector2 _minBounds;
    private Vector2 _maxBounds;

    #endregion

    #region Unity Methods

    private void Start()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();

        Bounds bounds = collider.bounds;

        _minBounds = bounds.min;
        _maxBounds = bounds.max;
    }

    #endregion

    #region Public Methods

    public Vector2 GetRandomSpawnPosition()
    {
        float x = Random.Range(_minBounds.x, _maxBounds.x);
        float y = Random.Range(_minBounds.y, _maxBounds.y);

        return new Vector2(x, y);
    }

    #endregion
}