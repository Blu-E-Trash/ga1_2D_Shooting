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

    public float nextFireTime => _nextFireTime;
    public float firerate => _fireRate;
    public bool ifAutoFire => _isAutoFire;
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
        Instantiate(BulletPrefab, FirePointL.position, Quaternion.identity);
        Instantiate(SubBulletPrefab, SubFirePointL.position, Quaternion.identity);

        Instantiate(BulletPrefab, FirePointR.position, Quaternion.identity);
        Instantiate(SubBulletPrefab, SubFirePointR.position, Quaternion.identity);
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