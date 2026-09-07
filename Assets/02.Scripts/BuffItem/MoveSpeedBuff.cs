using UnityEngine;

public class MoveSpeedBuff : Buff
{
    private float _speed = 0.5f;
    override public void ApplyBuff(GameObject player)
    {
        PlayerMove playermove = player.GetComponent<PlayerMove>();
        if (playermove != null)
        {
            playermove.SpeedBuff(_speed);
        }
    }
}