using UnityEngine;

public class EnemyTypeFollowPlayer : Enemy
{
    [SerializeField]
    private GameObject Player;

    override protected void Move()
    {
        if (Player != null)
        {
            Vector2 direction = Player.transform.position - transform.position;
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