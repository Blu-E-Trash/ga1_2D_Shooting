using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.name == "BossEnemy(Clone)")
            {
                return;
            }
        }
        Destroy(collision.gameObject);
    }
}
