using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected float _speed = 1f;

    protected virtual void Awake() { }

    protected virtual void Update()
    {
        Move();
    }

    protected virtual void Move()
    {
        Vector2 direction = new Vector2(0, -1);
        Vector2 normalizedSpeed = direction.normalized * _speed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
    }
}