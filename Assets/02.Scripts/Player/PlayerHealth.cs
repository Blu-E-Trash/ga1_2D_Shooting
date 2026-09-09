using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerHealth : MonoBehaviour
{
    private float _mMaxHealth = 100f;
    [SerializeField]
    private float _currentHealth;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip _hitSound;
    [SerializeField] private AudioClip[] _deathSounds;

    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _currentHealth = _mMaxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth > 0)
        {
            // 살아있을 때: 피격 사운드 재생
            if (_audioSource != null && _hitSound != null)
            {
                _audioSource.PlayOneShot(_hitSound);
            }
        }
        else
        {
            if (_deathSounds != null && _deathSounds.Length > 0)
            {
                int randomIndex = Random.Range(0, _deathSounds.Length);
                AudioSource.PlayClipAtPoint(_deathSounds[randomIndex], transform.position);
            }

            // 플레이어가 죽으면 게임 오버 연출 실행
            if (GameManager.Instance != null)
            {
                GameManager.Instance.GameOver();
            }

            Destroy(gameObject);
        }
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > _mMaxHealth)
        {
            _currentHealth = _mMaxHealth;
        }
    }
}