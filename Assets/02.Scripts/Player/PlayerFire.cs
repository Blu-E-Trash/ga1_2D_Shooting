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
    public float fireRate = 0.5f; // 발사 속도 (초 단위)

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            isAutoFire = !isAutoFire;
        }

        if (isAutoFire)
        {
            if (Time.time >= fireRate)
            {
                Fire();
                fireRate = Time.time + 0.5f;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Time.time >= fireRate)
            {
                Fire();
                fireRate = Time.time + 0.5f;
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
}
