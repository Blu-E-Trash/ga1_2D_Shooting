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

        // 화면 밖으로 날아간 총알 비활성화 (foreach에서 다시 찾을 수 있게 됨)
        if (transform.position.y >= _destroyPosY)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnSpawn()
    {
        PlaySound();
    }

    private void PlaySound()
    {
        _audioSource.pitch = UnityEngine.Random.Range(1f, 3f);
        _audioSource.Play();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(BulletDamage);

            // 적을 맞춘 총알 비활성화
            gameObject.SetActive(false);
        }
    }
}