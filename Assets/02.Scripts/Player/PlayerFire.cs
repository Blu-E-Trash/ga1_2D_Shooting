using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public Transform FirePointL;
    public Transform SubFirePointL;
    public Transform FirePointR;
    public Transform SubFirePointR;
    public GameObject BulletPrefab;
    public GameObject SubBulletPrefab;
    public bool isAutoFire = false;

    public float fireRate = 0.5f;

    private float nextFireTime = 0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isAutoFire = !isAutoFire;
        }

        if (isAutoFire)
        {
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time >= nextFireTime)
            {
                Fire();
                nextFireTime = Time.time + fireRate;
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
        fireRate -= amount;
        if (fireRate < 0.1f)
        {
            fireRate = 0.1f; // 최소 발사 속도 제한
        }
        Debug.Log($"현재 공격속도 간격: {fireRate}");
    }
}