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
        _maxHealth = _health; // 초기 인스펙터에 설정된 기본 체력 기억
    }

    // 다시 활성화될 때마다 체력을 리셋합니다.
    private void OnEnable()
    {
        _health = _maxHealth;
    }

    public void ApplyHealthMultiplier(float multiplier)
    {
        // Spawner에서 스폰 직후 호출되므로 배율에 맞춰 현재 체력이 세팅됩니다.
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

        // ObjectManager로 반환
        string poolTag = gameObject.name.Replace("(Clone)", "").Trim();

        if (ObjectManager.Instance != null)
        {
            ObjectManager.Instance.ReturnToPool(poolTag, gameObject);
        }
        else
        {
            gameObject.SetActive(false); // 매니저가 없을 때를 대비한 방어 코드
        }
    }
}