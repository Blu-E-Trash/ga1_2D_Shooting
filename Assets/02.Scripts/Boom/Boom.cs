using UnityEngine;

public class Boom : MonoBehaviour
{
    private float _timer = 0;
    private float _waitTime = 3f;

    [SerializeField]
    private float _bossDamage = 500f;

    private float _boomDamageMultiplier = 10f;

    private void OnEnable()
    {
        _timer = 0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    if (collision.gameObject.name.Contains("BossEnemy"))
                    {
                        float finalBossDamage = _bossDamage;
                        if (PlayerStatus.Instance != null)
                        {
                            finalBossDamage += (PlayerStatus.Instance.FinalBonusDamage * _boomDamageMultiplier);
                        }

                        enemyHealth.TakeDamage(finalBossDamage);
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
            ReturnToPool();
        }
    }

    private void ReturnToPool()
    {
        string poolTag = gameObject.name.Replace("(Clone)", "").Trim();
        if (ObjectManager.Instance != null)
        {
            ObjectManager.Instance.ReturnToPool(poolTag, gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}