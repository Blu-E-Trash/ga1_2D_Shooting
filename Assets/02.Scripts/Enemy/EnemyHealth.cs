using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float _health = 100f;
    private float _maxHealth;

    [Header("Reward Settings")]
    [SerializeField] private int _meritReward = 5; // 적 처치 시 획득할 공훈 수치

    private Animator _animator;
    private static readonly int IsHitHash = Animator.StringToHash("isHit");

    [SerializeField] private GameObject _deathEffectPrefab;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip _hitSound;

    private AudioSource _audioSource;
    private EnemyLoot _enemyLoot;

    // 💡 핵심: 중복 사망 처리를 막기 위한 상태 플래그
    private bool _isDead = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _enemyLoot = GetComponent<EnemyLoot>();
        _maxHealth = _health;
    }

    private void OnEnable()
    {
        _health = _maxHealth;
        _isDead = false; // 풀링에서 꺼내 재사용될 때 다시 살아나도록 초기화
    }

    public void ApplyHealthMultiplier(float multiplier)
    {
        _health = _maxHealth * multiplier;
    }

    public void TakeDamage(float damage)
    {
        // 이미 죽은 상태라면 추가 타격 무시 (중복 실행 방지)
        if (_isDead) return;

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
            Die(true);
        }
    }

    public void Die(bool shouldDropItem = true)
    {
        if (_isDead) return;
        _isDead = true;

        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddKillCount();
            if (shouldDropItem)
            {
                GameManager.Instance.AddMerit(_meritReward);
            }
        }

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