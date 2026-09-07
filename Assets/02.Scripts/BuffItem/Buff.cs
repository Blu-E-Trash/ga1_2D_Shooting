using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    GameObject player;

    protected float Speed = 3f;

    private float waitTime = 3f;
    private float timer = 0f;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (timer < waitTime)
        {
            timer += Time.deltaTime;
            return;
        }

        if (player != null)
        {
            Move();
        }
    }

    public abstract void ApplyBuff(GameObject player);

    protected void Move()
    {
        Vector2 direction = player.transform.position - transform.position;
        Vector2 normalizedSpeed = direction.normalized * Speed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyBuff(collision.gameObject);

            Destroy(gameObject);
        }
    }
}
