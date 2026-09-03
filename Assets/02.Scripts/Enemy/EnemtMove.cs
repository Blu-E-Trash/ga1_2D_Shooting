using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int EnemySpeed = 5;

    private void Update()
    {
        Vector2 direction = Vector2.down;

        Vector2 normalizedSpeed = direction.normalized * EnemySpeed;

        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);

        if (transform.position.y < -5.5f)
        {
            Destroy(gameObject);
        }
    }
}