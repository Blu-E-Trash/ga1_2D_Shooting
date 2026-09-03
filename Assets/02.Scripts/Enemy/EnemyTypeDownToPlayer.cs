using UnityEngine;
public class EnemyTypeDownToPlayer : Enemy
{
    private float _startPositionX = 0f;
    private void Start()
    {
        this.transform.position = new Vector2(_startPositionX, Random.Range(5.5f, 8.5f));
    }
}
