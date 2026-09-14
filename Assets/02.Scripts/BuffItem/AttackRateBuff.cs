using UnityEngine;

public class AttackRateBuff : Buff
{
    [SerializeField]
    private float _buffAmount = 0.05f;

    [SerializeField]
    private float _duration = 10f;

    public override void ApplyBuff(GameObject player)
    {
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.ApplyTempFireRateBuff(_buffAmount, _duration);
        }
    }
}