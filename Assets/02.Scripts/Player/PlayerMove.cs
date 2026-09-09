using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float Speed = 5f;
    private float _minPosX = -2.3f;
    private float _maxPosX = 2.3f;
    private float _minPosY = -4.68f;
    private float _maxPosY = 0f;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        SpeedChange();
    }

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal"); // 좌우 -1, 0, 1 반환
        float v = Input.GetAxisRaw("Vertical");

        if (_animator != null)
        {
            _animator.SetInteger("x", (int)h);
        }

        Vector2 direction = new Vector2(h, v);
        Vector2 normalizedSpeed = direction.normalized * Speed;

        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);

        if (transform.position.x < _minPosX)
        {
            transform.position = new Vector2(_maxPosX, transform.position.y);
        }
        if (transform.position.x > _maxPosX)
        {
            transform.position = new Vector2(_minPosX, transform.position.y);
        }
        if (transform.position.y < _minPosY)
        {
            transform.position = new Vector2(transform.position.x, _minPosY);
        }
        if (transform.position.y > _maxPosY)
        {
            transform.position = new Vector2(transform.position.x, _maxPosY);
        }
    }

    private void SpeedChange()
    {
        if (Input.GetKey(KeyCode.E))
        {
            Speed += 1f * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            if (Speed > 0)
            {
                Speed -= 1f * Time.deltaTime;
            }
        }
    }

    public void SpeedBuff(float amount)
    {
        Speed += amount;
        Debug.Log($"이동속도 {amount}증가");
    }
}