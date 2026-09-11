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

    // 다시 나올 때마다 대기 시간 타이머를 0으로 리셋
    private void OnEnable()
    {
        _timer = 0f;
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

            if (_pickupSound != null)
            {
                AudioSource.PlayClipAtPoint(_pickupSound, transform.position);
            }

            if (_getBuffEffect != null)
            {
                Instantiate(_getBuffEffect, transform.position, Quaternion.identity);
            }

            // 태그를 추출하여 ObjectManager로 반환
            string poolTag = gameObject.name.Replace("(Clone)", "").Trim();

            if (ObjectManager.Instance != null)
            {
                ObjectManager.Instance.ReturnToPool(poolTag, gameObject);
            }
            else
            {
                gameObject.SetActive(false); // 매니저 부재 시 방어 코드
            }
        }
    }
}