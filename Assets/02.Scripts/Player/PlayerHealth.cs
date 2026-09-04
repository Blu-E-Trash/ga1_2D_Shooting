using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float MaxHealth = 100f;
    [SerializeField]
    private float CurrentHealth;

    private void Start()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;
        Debug.Log(CurrentHealth.ToString());
        if (CurrentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
