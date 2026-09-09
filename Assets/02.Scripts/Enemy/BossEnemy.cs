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

    // 부모(Enemy)의 Move 함수를 덮어써서 보스만의 움직임을 만듭니다.
    protected override void Move()
    {
        // 1단계: 등장할 때 목표 위치(_stopPosY)까지 천천히 내려오기
        if (!_isIntroFinished)
        {
            transform.position += Vector3.down * _introSpeed * Time.deltaTime;

            // 목표 Y 좌표에 도달하면
            if (transform.position.y <= _stopPosY)
            {
                // 위치를 정확히 맞춰주고 상태를 전환합니다.
                transform.position = new Vector3(transform.position.x, _stopPosY, transform.position.z);
                _startX = transform.position.x; // 현재 위치를 좌우 이동의 중심축으로 저장
                _isIntroFinished = true; // 1단계 종료
            }
        }
        // 2단계: 상단에 자리 잡은 후 좌우로만 부드럽게 왕복 이동
        else
        {
            // Mathf.Sin(Time.time)은 시간이 지남에 따라 -1 ~ 1 사이의 값을 부드럽게 반복 반환합니다.
            float newX = _startX + Mathf.Sin(Time.time * _oscillationSpeed) * _oscillationWidth;

            // Y축(아래)으로는 움직이지 않고 오직 X축으로만 갱신합니다.
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        }
    }

    protected override void Die()
    {
        // 보스가 죽으면 게임 매니저에게 생존 시간 재개를 알림
        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndBossWave();
        }

        // 부모 클래스(Enemy)에 있는 원래 파괴 로직(이펙트 생성 및 Destroy) 실행
        base.Die();
    }
}