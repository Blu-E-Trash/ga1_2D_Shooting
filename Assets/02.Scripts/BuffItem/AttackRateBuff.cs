using UnityEngine;

public class AttackRateBuff : Buff
{
    private float _buffamount = 0.05f;
    public override void ApplyBuff(GameObject player)
    {
        PlayerFire playerFire = player.GetComponent<PlayerFire>();
        if (playerFire != null)
        {
            playerFire.FireRateBuff(_buffamount); // 발사 속도 증가
        }
    }
}