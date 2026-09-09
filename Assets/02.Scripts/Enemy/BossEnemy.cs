using UnityEngine;

public class BossEnemy : Enemy
{
    [Header("Boss Movement Settings")]
    [SerializeField]
    private float _stopPosY = 3.5f;
    [SerializeField]
    private float _introSpeed = 2f; // 처음 등장할 때 아래로 내려오는 속도
    [SerializeField]
    private float _oscillationSpeed = 1f; // 좌우 이동 속도
    [SerializeField]
    private float _oscillationWidth = 1f; // 좌우 이동 폭

    private bool _isIntroFinished = false;
    [SerializeField]
    private float _startX; // 좌우 이동의 기준점이 될 처음 X 좌표

    protected override void Move()
    {
        // 등장할 때 목표 위치까지 천천히 내려오기
        if (!_isIntroFinished)
        {
            transform.position += Vector3.down * _introSpeed * Time.deltaTime;

            // 목표 Y 좌표에 도달하면
            if (transform.position.y <= _stopPosY)
            {
                // 위치를 정확히 맞춰주고 상태를 전환
                transform.position = new Vector3(transform.position.x, _stopPosY, transform.position.z);
                _startX = transform.position.x; // 현재 위치를 좌우 이동의 중심축으로 저장
                _isIntroFinished = true;
            }
        }
        // 상단에 자리 잡은 후 좌우로만 부드럽게 왕복 이동
        else
        {
            float newX = _startX + Mathf.Sin(Time.time * _oscillationSpeed) * _oscillationWidth;

            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    protected override void Die()
    {
        if (BossManager.Instance != null)
        {
            BossManager.Instance.EndBossWave();
        }

        base.Die();
    }
}