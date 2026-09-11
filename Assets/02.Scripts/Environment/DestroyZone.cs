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
        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
            return;
        }
        Destroy(collision.gameObject);
    }
}
