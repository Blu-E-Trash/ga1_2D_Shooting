using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float BulletDamage = 10f;
    public float BulletSpeed = 20f;

    [SerializeField]
    AudioSource _audioSource;

    [SerializeField]
    private float _destroyPosY = 10f; // 총알이 회수될 화면 최상단 Y 좌표

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        Vector2 direction = Vector2.up;
        transform.position += (Vector3)direction * BulletSpeed * Time.deltaTime;

        // 화면 밖으로 날아간 총알 회수
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
            enemyHealth.TakeDamage(BulletDamage);

            // 적을 맞춘 총알 회수
            ReturnToPool();
        }
    }

    // 통합 매니저로 반환
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