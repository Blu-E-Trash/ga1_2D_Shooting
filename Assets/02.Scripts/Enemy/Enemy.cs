using UnityEngine;

abstract public class Enemy : MonoBehaviour
{
    public int Speed = 1;
    public float Health = 100f;
    protected float _minPosY = -5.5f;

    private void Update()
    {
        Move();
    }
    virtual protected void Move()
    {
        Vector2 direction = new Vector2(0, -1);
        Vector2 normalizedSpeed = direction.normalized * Speed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
        if (transform.position.y < _minPosY)
        {
            Destroy(gameObject);
        }
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
}