using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float MaxHealth = 100f;
    [SerializeField]
    private float _currentHealth;

    public float Health => _currentHealth;


    private void Start()
    {
        _currentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        Debug.Log(_currentHealth.ToString());
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void Heal(float amount)
    {
        _currentHealth += amount;
        if (_currentHealth > MaxHealth)
        {
            _currentHealth = MaxHealth;
        }
        Debug.Log($"현재 체력 {_currentHealth}");
    }
}
