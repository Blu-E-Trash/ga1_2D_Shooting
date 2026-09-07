using UnityEngine;

public class EnemyTypeDownToPlayer : Enemy
{
    private Vector2 _moveDirection;

    private GameObject _player;

    override protected void Awake()
    {
        base.Awake();
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private void Start()
    {
        Vector2 direction = _player.transform.position - this.transform.position;
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