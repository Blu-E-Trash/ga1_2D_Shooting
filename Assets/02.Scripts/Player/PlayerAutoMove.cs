using UnityEngine;

public class PlayerAutoMove : MonoBehaviour
{
    [SerializeField] private int _stopTrackingY = 2;
    [SerializeField] private float _findTargetCooldown = 0.5f;

    private GameObject _target = null;
    private float _searchTimer = 0f;
    private PlayerMove _playerMove;

    private void Awake()
    {
        _playerMove = GetComponent<PlayerMove>();
        if (_playerMove == null)
        {
            return;
        }
    }

    private void Update()
    {
        if (_target == null || !_target.activeInHierarchy || _target.transform.position.y < -_stopTrackingY)
        {
            _searchTimer += Time.deltaTime;
            if (_searchTimer >= _findTargetCooldown)
            {
                FindNearestTarget();
                _searchTimer = 0f;
            }
        }
        else
        {
            Move();
        }
    }

    private void Move()
    {
        if (_target == null || !_target.activeInHierarchy) return;

        Vector3 diff = _target.transform.position - transform.position;
        Vector3 direction;

        if (diff.y >= 3f)
        {
            direction = diff.normalized;
        }
        else
        {
            direction = new Vector3(diff.x, -1f, 0).normalized;
        }

        // PlayerMove의 Speed 값을 가져와서 이동에 반영
        float currentSpeed = _playerMove != null ? _playerMove.Speed : 5f;
        transform.position += direction * currentSpeed * Time.deltaTime;
    }

    private void FindNearestTarget()
    {
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Enemy");
        if (targets.Length == 0)
        {
            _target = null;
            return;
        }

        _target = null;
        float minDistance = float.MaxValue;

        foreach (GameObject enemy in targets)
        {
            if (!enemy.activeInHierarchy) continue;

            if (enemy.transform.position.y < -_stopTrackingY) continue;

            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                _target = enemy;
            }
        }
    }
}