using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
abstract public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int _speed = 1;
    [SerializeField]
    protected float _health = 100f;
    protected float _minPosY = -5.5f;
    [SerializeField]
    protected float _damage = 10f;

    [SerializeField]
    private GameObject[] _buffItem;
    [SerializeField]
    private int _dropRate;

    private Animator _animator;
    private static readonly int IsHitHash = Animator.StringToHash("isHit");

    [SerializeField]
    private GameObject _deathEffectPrefab;
    private float _maxHealth;

    [Header("Audio Settings")]
    [SerializeField]
    protected AudioClip _hitSound; // 피격 및 사망 시 재생할 사운드

    protected AudioSource _audioSource;

    virtual protected void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _maxHealth = _health;
    }

    protected void Update()
    {
        Move();
    }

    public void ApplyHealthMultiplier(float multiplier)
    {
        _health = _maxHealth * multiplier;
    }

    virtual protected void Move()
    {
        Vector2 direction = new Vector2(0, -1);
        Vector2 normalizedSpeed = direction.normalized * _speed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
    }

    public void TakeDamage(float damage)
    {
        _animator?.SetTrigger(IsHitHash);
        _health -= damage;

        if (_health > 0)
        {
            // 살아있을 때: 피격 사운드를 겹쳐서 자연스럽게 재생
            if (_audioSource != null && _hitSound != null)
            {
                _audioSource.PlayOneShot(_hitSound);
            }
        }
        else
        {
            // 체력이 0이 되어 죽을 때만 킬 카운트 증가
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddKillCount();
            }

            TryDropItem();
            Die();
        }
    }

    protected virtual void Die()
    {
        if (_hitSound != null)
        {
            AudioSource.PlayClipAtPoint(_hitSound, transform.position);
        }

        if (_deathEffectPrefab != null)
        {
            Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }

    public void Kill()
    {
        TryDropItem();
        Die();
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
            Die();
        }
    }

    private void TryDropItem()
    {
        if (_buffItem == null || _buffItem.Length == 0) return;

        int randomChance = Random.Range(0, 100);
        if (randomChance < _dropRate)
        {
            int index = Random.Range(0, _buffItem.Length);

            Instantiate(_buffItem[index], transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("생성 실패");
        }
    }
}