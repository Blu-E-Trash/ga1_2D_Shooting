using UnityEngine;

public class Bullet : MonoBehaviour
{
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
}