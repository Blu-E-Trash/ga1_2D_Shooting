using UnityEngine;

[RequireComponent(typeof(Animator))]
abstract public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int _speed = 1;
    protected float _health = 100f;
    protected float _minPosY = -5.5f;
    [SerializeField]
    protected float _damage = 10f;

    [SerializeField]
    private GameObject[] _buffItem;
    [SerializeField]
    private int _dropRate;

    private Animator _animator;
    private static readonly int IsHitHash = Animator.StringToHash("isHit");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
    }
    virtual protected void Move()
    {
        Vector2 direction = new Vector2(0, -1);
        Vector2 normalizedSpeed = direction.normalized * _speed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
    }
    public void TakeDamage(float damage)
    {
        _animator?.SetTrigger(IsHitHash);
        _health -= damage;
        if (_health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        TryDropItem();
        Debug.Log("Enemy Destroyed");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
    }
    private void TryDropItem()
    {
        if (_buffItem == null || _buffItem.Length == 0) return;

        int randomChance = Random.Range(0, 100);
        if (randomChance < _dropRate)
        {
            int index = Random.Range(0, _buffItem.Length);

            Instantiate(_buffItem[index], transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("생성 실패");
        }
    }
}