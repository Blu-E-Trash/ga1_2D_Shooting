using UnityEngine;

public class PlayerMove : MonoBehaviour
{
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
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            if (_animator != null) _animator.SetInteger("x", 0);
            return;
        }

        if (PlayerStatus.Instance != null && PlayerStatus.Instance.IsAutoMode)
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        float h = SimpleInput.GetAxisRaw("Horizontal");
        float v = SimpleInput.GetAxisRaw("Vertical");

        if (_animator != null)
        {
            _animator.SetInteger("x", (int)h);
        }

        Vector2 direction = new Vector2(h, v);

        float currentSpeed = PlayerStatus.Instance != null ? PlayerStatus.Instance.FinalMoveSpeed : 5f;
        Vector2 normalizedSpeed = direction.normalized * currentSpeed;

        transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);

        if (transform.position.x < _minPosX) transform.position = new Vector2(_maxPosX, transform.position.y);
        if (transform.position.x > _maxPosX) transform.position = new Vector2(_minPosX, transform.position.y);
        if (transform.position.y < _minPosY) transform.position = new Vector2(transform.position.x, _minPosY);
        if (transform.position.y > _maxPosY) transform.position = new Vector2(transform.position.x, _maxPosY);
    }
}