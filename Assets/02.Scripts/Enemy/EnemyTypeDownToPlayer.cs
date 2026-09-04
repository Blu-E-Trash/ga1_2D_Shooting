using UnityEngine;

public class EnemyTypeDownToPlayer : Enemy
{
    private Vector2 _moveDirection;

    private void Start()
    {
        GameObject player = FindAnyObjectByType<PlayerMove>().gameObject;

        _moveDirection = (player.transform.position - this.transform.position).normalized;
    }

    override protected void Move()
    {
        Vector2 normalizedSpeed = _moveDirection * Speed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
    }
}