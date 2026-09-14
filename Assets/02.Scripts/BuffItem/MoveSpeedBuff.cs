using UnityEngine;

public class MoveSpeedBuff : Buff
{
    [SerializeField]
    private float _buffAmount = 0.5f;

    [SerializeField]
    private float _duration = 10f;

    public override void ApplyBuff(GameObject player)
    {
        if (PlayerStatus.Instance != null)
        {
            PlayerStatus.Instance.ApplyTempMoveSpeedBuff(_buffAmount, _duration);
        }
    }
}