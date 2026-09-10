using UnityEngine;

public class Boom : MonoBehaviour
{
    private float _timer = 0;
    private float _waitTime = 3f;

    [SerializeField]
    private float _bossDamage = 500f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    if (collision.gameObject.name == "BossEnemy(Clone)")
                    {
                        enemyHealth.TakeDamage(_bossDamage);
                        return;
                    }

                    enemyHealth.Die();
                }
            }
        }
    }

    private void Update()
    {
        if (_timer < _waitTime)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}