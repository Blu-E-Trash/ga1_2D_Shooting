using UnityEngine;
public class EnemyType1 : Enemy
{
    private void Start()
    {
        this.transform.position = new Vector2(0f, Random.Range(5.5f, 8.5f));
    }
}
