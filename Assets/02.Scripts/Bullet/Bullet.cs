using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bulletSpeed = 20f;
    
    void Update()
    {
        Vector2 direction = Vector2.up;

        transform.position += (Vector3) direction * bulletSpeed * Time.deltaTime;

        if(transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }
}
