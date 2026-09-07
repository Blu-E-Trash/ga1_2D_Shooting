using UnityEngine;

public class EnemyTypeFollowPlayer : Enemy
{
    [SerializeField]
    private GameObject _player;

    private void Awake()
    {
        if (_player == null)
        {
            Debug.LogError($"{gameObject.name}에 Player가 할당되지 않았습니다.");
        }
    }
    override protected void Move()
    {
        if (_player != null)
        {
            Vector2 direction = _player.transform.position - transform.position;
            Vector2 normalizedSpeed = direction.normalized * _speed;
            transform.position += (Vector3)(normalizedSpeed * Time.deltaTime);

            Rotate(direction);
        }
    }
    private void Rotate(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + 90f);
    }
}