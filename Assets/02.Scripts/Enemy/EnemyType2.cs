using UnityEngine;

public class EnemyType2 : Enemy
{
    public GameObject Player;
    private void Start()
    {
        Player = FindAnyObjectByType<PlayerMove>().gameObject;
    }
    private void Update()
    {
        EnemyMove();
    }

    override protected void EnemyMove()
    {
        Vector2 direction = Player.transform.position - transform.position;
        Vector2 normalizedSpeed = direction.normalized * EnemySpeed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
        if (transform.position.y < -5.5f)
        {
            Destroy(gameObject);
        }
    }
}
