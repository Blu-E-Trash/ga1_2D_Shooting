using UnityEngine;

public class EnemyTypeDownToPlayer : Enemy
{
    private Vector2 _moveDirection;

    [SerializeField]
    private GameObject player;

    private void Start()
    {
        Vector2 direction = player.transform.position - this.transform.position;
        _moveDirection = direction.normalized;

        RotateTowards(direction);
    }

    override protected void Move()
    {
        Vector2 normalizedSpeed = _moveDirection * _speed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
    }

    private void RotateTowards(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}