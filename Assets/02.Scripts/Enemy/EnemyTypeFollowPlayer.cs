using UnityEngine;

public class EnemyTypeFollowPlayer : Enemy
{
    private GameObject _player;

    override protected void Awake()
    {
        base.Awake();
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag("Player");
        }
    }
    override protected void Move()
    {
        if (_player != null)
        {
            Vector2 direction = _player.transform.position - transform.position;
            Vector2 normalizedSpeed = direction.normalized * _speed;
            transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);

            Rotate(direction);
        }
    }
    private void Rotate(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}