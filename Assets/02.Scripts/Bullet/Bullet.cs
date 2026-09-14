using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletDamage = 10f;
    public float BulletSpeed = 20f;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    private float _destroyPosY = 10f;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector2 direction = Vector2.up;
        transform.position += (Vector3)direction * BulletSpeed * Time.deltaTime;

        if (transform.position.y >= _destroyPosY)
        {
            ReturnToPool();
        }
    }

    public void OnSpawn()
    {
        PlaySound();
    }

    private void PlaySound()
    {
        if (_audioSource != null)
        {
            _audioSource.pitch = UnityEngine.Random.Range(1f, 3f);
            _audioSource.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out EnemyHealth enemyHealth))
        {
            float finalDamage = BulletDamage;
            if (PlayerStatus.Instance != null)
            {
                finalDamage += PlayerStatus.Instance.FinalBonusDamage;
            }

            enemyHealth.TakeDamage(finalDamage);
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