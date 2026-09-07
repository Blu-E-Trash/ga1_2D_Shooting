using UnityEngine;

public class EnemyTypeFollowPlayer : Enemy
{
    public GameObject Player;

    private void Start()
    {
        Player = FindAnyObjectByType<PlayerMove>().gameObject;
    }

    override protected void Move()
    {
        if (Player != null)
        {
            Vector2 direction = Player.transform.position - transform.position;
            Vector2 normalizedSpeed = direction.normalized * Speed;
            transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
        }
    }
}