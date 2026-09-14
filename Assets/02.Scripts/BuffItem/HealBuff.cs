using UnityEngine;

public class HealBuff : Buff
{
    [SerializeField]
    private float _healAmount = 20f;

    public override void ApplyBuff(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(_healAmount);
        }
    }
}