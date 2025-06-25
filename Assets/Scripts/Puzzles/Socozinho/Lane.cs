using UnityEngine;

public class Lane : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _length = 5.0f;

    private Vector2 _pointA;
    private Vector2 _pointB;

    private bool _isOccupied;

    #endregion

    #region Properties

    public Vector2 PointA => _pointA;
    public Vector2 PointB => _pointB;

    public bool IsOccupied => _isOccupied;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _pointA = (Vector2)transform.position + Vector2.left * (_length * 0.5f);
        _pointB = (Vector2)transform.position + Vector2.right * (_length * 0.5f);
    }

#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        Gizmos.color = _isOccupied ? Color.red : Color.green;

        Vector2 pointA = (Vector2)transform.position + Vector2.left * (_length * 0.5f);
        Vector2 pointB = (Vector2)transform.position + Vector2.right * (_length * 0.5f);

        Gizmos.DrawLine(pointA, pointB);
        Gizmos.DrawSphere(pointA, 0.1f);
        Gizmos.DrawSphere(pointB, 0.1f);
    }

#endif

    #endregion

    #region Public Methods

    public Vector2 GetRandomPosition()
    {
        return Vector2.Lerp(_pointA, _pointB, Random.Range(0.0f, 1.0f));
    }

    public void SetOccupied()
    {
        _isOccupied = true;
    }

    public void SetAvailable()
    {
        _isOccupied = false;
    }

    #endregion
}