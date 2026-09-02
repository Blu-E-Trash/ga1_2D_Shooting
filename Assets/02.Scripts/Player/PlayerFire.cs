using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    public Transform bulletSpawnPoint;
    public GameObject BulletPrefab;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Instantiate는 프리펩을 복사해서 게임 오브젝트를 생성하고 씬에 넣어주는 기능
            GameObject bullet = Instantiate(BulletPrefab);
            bullet.transform.position = bulletSpawnPoint.position;
        }
    }
}
