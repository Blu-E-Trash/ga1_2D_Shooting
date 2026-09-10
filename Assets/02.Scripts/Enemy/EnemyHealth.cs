using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    private float _maxHealth;

    private Animator _animator;
    private static readonly int IsHitHash = Animator.StringToHash("isHit");

    [SerializeField] private GameObject _deathEffectPrefab;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip _hitSound;

    private AudioSource _audioSource;
    private EnemyLoot _enemyLoot;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _enemyLoot = GetComponent<EnemyLoot>();
        _maxHealth = _health;
    }

    public void ApplyHealthMultiplier(float multiplier)
    {
        _health = _maxHealth * multiplier;
    }

    public void TakeDamage(float damage)
    {
        _animator?.SetTrigger(IsHitHash);
        _health -= damage;

        if (_health > 0)
        {
            if (_audioSource != null && _hitSound != null)
            {
                _audioSource.PlayOneShot(_hitSound);
            }
        }
        else
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddKillCount();
            }

            Die(true);
        }
    }

    public void Die(bool shouldDropItem = true)
    {
        if (_hitSound != null)
        {
            AudioSource.PlayClipAtPoint(_hitSound, transform.position);
        }

        if (_deathEffectPrefab != null)
        {
            Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
        }

        if (shouldDropItem && _enemyLoot != null)
        {
            _enemyLoot.TryDropItem();
        }

        if (GetComponent<BossEnemy>() != null)
        {
            if (BossManager.Instance != null) BossManager.Instance.EndBossWave();
        }

        Destroy(gameObject);
    }
}