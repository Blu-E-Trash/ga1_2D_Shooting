using UnityEngine;

public class DestroyZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == null) return;

        if (collision.gameObject.name == "BossEnemy(Clone)")
        {
            return;
        }

        if (collision.gameObject.CompareTag("Bullet"))
        {
            collision.gameObject.SetActive(false);
            return;
        }

        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            string poolTag = collision.gameObject.name.Replace("(Clone)", "").Trim();

            if (ObjectManager.Instance != null)
            {
                ObjectManager.Instance.ReturnToPool(poolTag, collision.gameObject);
            }
            else
            {
                collision.gameObject.SetActive(false);
            }
            return;
        }

        Destroy(collision.gameObject);
    }
}