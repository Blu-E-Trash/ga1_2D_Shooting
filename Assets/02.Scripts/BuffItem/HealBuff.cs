using UnityEngine;

public class HealBuff : Buff
{
    private float _heal = 20f;
    public override void ApplyBuff(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(_heal);
        }
    }
}
