using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    GameObject _player;
    [SerializeField]
    private AudioClip _pickupSound;
    [SerializeField]
    private GameObject _getBuffEffect;

    [SerializeField]
    protected float _buffItemSpeed;

    [SerializeField]
    private float _waitTime;
    private float _timer = 0f;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (_timer < _waitTime)
        {
            _timer += Time.deltaTime;
            return;
        }

        if (_player != null)
        {
            Move();
        }
    }

    public abstract void ApplyBuff(GameObject player);

    protected void Move()
    {
        if (_player == null) return;
        Vector2 direction = _player.transform.position - transform.position;
        Vector2 normalizedSpeed = direction.normalized * _buffItemSpeed;
        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ApplyBuff(collision.gameObject);
            AudioSource.PlayClipAtPoint(_pickupSound, transform.position);
            Instantiate(_getBuffEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
