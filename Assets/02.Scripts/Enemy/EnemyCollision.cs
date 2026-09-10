using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private float _damage = 10f;
    private EnemyHealth _enemyHealth;

    private void Awake()
    {
        _enemyHealth = GetComponent<EnemyHealth>();
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

            if (_enemyHealth != null)
            {
                _enemyHealth.Die(false);
            }
        }
    }
}