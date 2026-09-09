using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float _mMaxHealth = 100f;
    [SerializeField]
    private float _currentHealth;

    private void Start()
    {
        _currentHealth = _mMaxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
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