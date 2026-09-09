using UnityEngine;

public class Boom : MonoBehaviour
{
    private float _timer = 0;
    private float _waitTime = 3f;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                if (collision.gameObject.name == "BossEnemy(Clone)")
                {
                    enemy.TakeDamage(500);
                    return;
                }
                enemy.Kill();
            }
        }
    }
    private void Update()
    {
        if (_timer < _waitTime)
        {
            _timer += Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
