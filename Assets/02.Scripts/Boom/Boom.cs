using UnityEngine;

public class Boom : MonoBehaviour
{
    private float _timer = 0;
    private float _waitTime = 3;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.CompareTag("Enemy"))
            {
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy.TryDropItem();
                enemy.Die();
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
            Destroy(gameObject);
    }
}
