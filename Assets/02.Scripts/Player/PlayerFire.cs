using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public Transform FirePointL;
    public Transform SubFirePointL;
    public Transform FirePointR;
    public Transform SubFirePointR;
    public GameObject BulletPrefab;
    public GameObject SubBulletPrefab;
    private bool _isAutoFire = false;

    private float _fireRate = 0.5f;

    private float _nextFireTime = 0f;

    public float NextFireTime => _nextFireTime;
    public float Firerate => _fireRate;
    public bool IfAutoFire => _isAutoFire;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _isAutoFire = !_isAutoFire;
        }

        if (_isAutoFire)
        {
            if (Time.time >= _nextFireTime)
            {
                Fire();
                _nextFireTime = Time.time + _fireRate;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time >= _nextFireTime)
            {
                Fire();
                _nextFireTime = Time.time + _fireRate;
            }
        }
    }

    private void Fire()
    {
        Bullet leftBullet = BulletPool.Instance.ReturnBullet();
        if (leftBullet != null)
        {
            leftBullet.transform.position = FirePointL.position;
        }

        Bullet rightBullet = BulletPool.Instance.ReturnBullet();
        if (rightBullet != null)
        {
            rightBullet.transform.position = FirePointR.position;
        }

        Bullet subLeftBullet = BulletPool.Instance.ReturnSubBullet();
        if (subLeftBullet != null)
        {
            subLeftBullet.transform.position = SubFirePointL.position;
        }

        Bullet subRightBullet = BulletPool.Instance.ReturnSubBullet();
        if (subRightBullet != null)
        {
            subRightBullet.transform.position = SubFirePointR.position;
        }
    }

    public void FireRateBuff(float amount)
    {
        _fireRate -= amount;
        if (_fireRate < 0.1f)
        {
            _fireRate = 0.1f; // 최소 발사 속도 제한
        }
        Debug.Log($"현재 공격속도 간격: {_fireRate}");
    }
}