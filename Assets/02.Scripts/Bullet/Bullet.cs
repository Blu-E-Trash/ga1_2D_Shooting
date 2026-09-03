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
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(this.gameObject);

        if (other.gameObject.TryGetComponent(out Enemy enemy))
        {
            enemy.TakeDamage(BulletDamage);
            Debug.Log("Enemy Destroyed");
        }
    }
}