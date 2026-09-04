using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    public int Speed = 1;
    public float Health = 100f;
    protected float _minPosY = -5.5f;
    [SerializeField]
    protected float _damage = 10f;

    private void Update()
    {
        Move();
    }
    virtual protected void Move()
    {
        Vector2 direction = new Vector2(0, -1);
        Vector2 normalizedSpeed = direction.normalized * Speed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
    }
    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            Destroy(gameObject);
            Debug.Log("Enemy Destroyed");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
    }
}