using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletDamage = 10f;
    public float BulletSpeed = 20f;

    void Update()
    {
        Vector2 direction = Vector2.up;

        transform.position += (Vector3)direction * BulletSpeed * Time.deltaTime;

        if (transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(this.gameObject);

        if (collision.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(BulletDamage);
            Debug.Log("Enemy Destroyed");
        }
    }
}