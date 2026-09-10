using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss Movement Settings")]
    [SerializeField] private float _stopPosY = 3.5f;
    [SerializeField] private float _introSpeed = 2f;
    [SerializeField] private float _oscillationSpeed = 1f;
    [SerializeField] private float _oscillationWidth = 1f;

    private bool _isIntroFinished = false;
    [SerializeField] private float _startX;

    protected override void Move()
    {
        if (!_isIntroFinished)
        {
            transform.position += Vector3.down * _introSpeed * Time.deltaTime;

            if (transform.position.y <= _stopPosY)
            {
                transform.position = new Vector3(transform.position.x, _stopPosY, transform.position.z);
                _startX = transform.position.x;
                _isIntroFinished = true;
            }
        }
        else
        {
            float newX = _startX + Mathf.Sin(Time.time * _oscillationSpeed) * _oscillationWidth;
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }
}