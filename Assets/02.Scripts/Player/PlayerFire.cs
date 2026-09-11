using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public Transform FirePointL;
    public Transform SubFirePointL;
    public Transform FirePointR;
    public Transform SubFirePointR;

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
        GameObject leftBullet = ObjectManager.Instance.SpawnFromPool("Bullet", FirePointL.position, Quaternion.identity);
        if (leftBullet != null) leftBullet.GetComponent<Bullet>()?.OnSpawn();

        GameObject rightBullet = ObjectManager.Instance.SpawnFromPool("Bullet", FirePointR.position, Quaternion.identity);
        if (rightBullet != null) rightBullet.GetComponent<Bullet>()?.OnSpawn();

        GameObject subLeftBullet = ObjectManager.Instance.SpawnFromPool("SubBullet", SubFirePointL.position, Quaternion.identity);
        if (subLeftBullet != null) subLeftBullet.GetComponent<Bullet>()?.OnSpawn();

        GameObject subRightBullet = ObjectManager.Instance.SpawnFromPool("SubBullet", SubFirePointR.position, Quaternion.identity);
        if (subRightBullet != null) subRightBullet.GetComponent<Bullet>()?.OnSpawn();
    }

    public void FireRateBuff(float amount)
    {
        _fireRate -= amount;
        if (_fireRate < 0.1f)
        {
            _fireRate = 0.1f;
        }
        Debug.Log($"현재 공격속도 간격: {_fireRate}");
    }
}