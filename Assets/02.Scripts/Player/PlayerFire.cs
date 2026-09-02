using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public Transform FirePointL;
    public Transform FirePointR;
    public GameObject BulletPrefab;

    public float fireRate = 0.5f; // 발사 속도 (초 단위)

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(Time.time >= fireRate)
            {
                Fire();
                fireRate = Time.time + 0.5f; // 다음 발사 가능 시간 설정
            }

        }
    }
    private void Fire()
    {            //Instantiate는 프리펩을 복사해서 게임 오브젝트를 생성하고 씬에 넣어주는 기능
        GameObject bulletL = Instantiate(BulletPrefab);
        bulletL.transform.position = FirePointL.position;

        GameObject bulletR = Instantiate(BulletPrefab);
        bulletR.transform.position = FirePointR.position;
    }
}
