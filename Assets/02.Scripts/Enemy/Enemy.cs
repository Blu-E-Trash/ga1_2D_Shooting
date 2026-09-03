using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    public int EnemySpeed = 1;
    public float EnemyHealth = 100f;

    private void Update()
    {
        EnemyMove();
    }
    virtual protected void EnemyMove()
    {
        Vector2 direction = new Vector2(0, -1);
        Vector2 normalizedSpeed = direction.normalized * EnemySpeed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
        if (transform.position.y < -5.5f)
        {
            Destroy(gameObject);
        }
    }
    public void TakeDamage(float damage)
    {
        EnemyHealth -= damage;
        if (EnemyHealth <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy Destroyed");
        }
    }
}