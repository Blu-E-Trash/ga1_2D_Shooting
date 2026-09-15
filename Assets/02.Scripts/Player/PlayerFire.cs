using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public Transform FirePointL;
    public Transform SubFirePointL;
    public Transform FirePointR;
    public Transform SubFirePointR;

    private float _nextFireTime = 0f;
    private bool _isFireButtonPressed = false;

    public float NextFireTime => _nextFireTime;

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            return;
        }

        bool isAuto = PlayerStatus.Instance != null && PlayerStatus.Instance.IsAutoMode;

        float currentFireRate = PlayerStatus.Instance != null ? PlayerStatus.Instance.FinalFireRate : 0.5f;

        if (isAuto)
        {
            if (Time.time >= _nextFireTime)
            {
                Fire();
                _nextFireTime = Time.time + currentFireRate;
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space) || _isFireButtonPressed)
            {
                if (Time.time >= _nextFireTime)
                {
                    Fire();
                    _nextFireTime = Time.time + currentFireRate;
                }
            }
        }
    }

    private void Fire()
    {
        GameObject leftBullet = ObjectManager.Instance.SpawnFromPool("Bullet", FirePointL.position, Quaternion.identity);
        if (leftBullet != null) leftBullet.GetComponent<Bullet>()?.OnSpawn();

        GameObject rightBullet = ObjectManager.Instance.SpawnFromPool("Bullet", FirePointR.position, Quaternion.identity);
        if (rightBullet != null) rightBullet.GetComponent<Bullet>()?.OnSpawn();

        GameObject subLeftBullet = ObjectManager.Instance.SpawnFromPool("SubBullet", SubFirePointL.position, Quaternion.identity);
        if (subLeftBullet != null) subLeftBullet.GetComponent<Bullet>()?.OnSpawn();

        GameObject subRightBullet = ObjectManager.Instance.SpawnFromPool("SubBullet", SubFirePointR.position, Quaternion.identity);
        if (subRightBullet != null) subRightBullet.GetComponent<Bullet>()?.OnSpawn();
    }

    public void OnFireButtonDown()
    {
        _isFireButtonPressed = true;
    }

    public void OnFireButtonUp()
    {
        _isFireButtonPressed = false;
    }
}