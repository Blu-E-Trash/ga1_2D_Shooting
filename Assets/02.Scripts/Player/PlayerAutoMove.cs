using UnityEngine;

public class PlayerAutoMove : MonoBehaviour
{
    [SerializeField] private int _stopTrackingY = 2;
    [SerializeField] private float _findTargetCooldown = 0.25f;
    [SerializeField] private float _evadeDistance = 2f;

    private GameObject _target = null;
    private float _searchTimer = 0f;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            _target = null;
            if (_animator != null) _animator.SetInteger("x", 0);
            return;
        }

        if (PlayerStatus.Instance == null || !PlayerStatus.Instance.IsAutoMode)
        {
            _target = null;
            return;
        }

        _searchTimer += Time.deltaTime;
        if (_searchTimer >= _findTargetCooldown)
        {
            FindNearestTarget();
            _searchTimer = 0f;
        }

        Move();
    }

    private void Move()
    {
        if (_target == null || !_target.activeInHierarchy)
        {
            if (_animator != null) _animator.SetInteger("x", 0);
            return;
        }

        float distanceToTarget = Vector2.Distance(transform.position, _target.transform.position);
        Vector3 diff = _target.transform.position - transform.position;
        Vector3 direction;

        if (distanceToTarget <= _evadeDistance)
        {
            direction = new Vector3(diff.x, -1f, 0).normalized;
        }
        else
        {
            direction = diff.normalized;
        }

        if (_animator != null)
        {
            if (direction.x > 0.1f) _animator.SetInteger("x", 1);
            else if (direction.x < -0.1f) _animator.SetInteger("x", -1);
            else _animator.SetInteger("x", 0);
        }

        float currentSpeed = PlayerStatus.Instance != null ? PlayerStatus.Instance.FinalMoveSpeed : 5f;
        transform.position += direction * currentSpeed * Time.deltaTime;

        float minPosX = -2.3f;
        float maxPosX = 2.3f;
        float minPosY = -4.68f;
        float maxPosY = 0f;

        if (transform.position.x < minPosX) transform.position = new Vector2(maxPosX, transform.position.y);
        if (transform.position.x > maxPosX) transform.position = new Vector2(minPosX, transform.position.y);
        if (transform.position.y < minPosY) transform.position = new Vector2(transform.position.x, minPosY);
        if (transform.position.y > maxPosY) transform.position = new Vector2(transform.position.x, maxPosY);
    }

    private void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        if (targets.Length == 0)
        {
            _target = null;
            return;
        }

        GameObject closestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (GameObject enemy in targets)
        {
            if (!enemy.activeInHierarchy) continue;
            if (enemy.transform.position.y < -_stopTrackingY) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }
        }

        _target = closestEnemy;
    }
}