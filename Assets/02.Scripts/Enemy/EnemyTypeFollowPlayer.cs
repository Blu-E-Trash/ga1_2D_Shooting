using UnityEngine;

public class EnemyTypeFollowPlayer : Enemy
{
    public GameObject Player;
    private void Start()
    {
        Player = FindAnyObjectByType<PlayerMove>().gameObject;
    }
    private void Update()
    {
        Move();
    }

    override protected void Move()
    {
        Vector2 direction = Player.transform.position - transform.position;
        Vector2 normalizedSpeed = direction.normalized * Speed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
    }
}
