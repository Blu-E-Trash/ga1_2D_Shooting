using UnityEngine;

public class BulletPool : MonoBehaviour
{
    private static BulletPool _instance = null;
    public static BulletPool Instance => _instance;

    [Header("메인 총알 설정")]
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private int _poolSize = 50;
    private Bullet[] _pool;

    [Header("서브 총알 설정")]
    [SerializeField] private Bullet _subBulletPrefab;
    [SerializeField] private int _subPoolSize = 50;
    private Bullet[] _subPool;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        _pool = new Bullet[_poolSize];
        for (int i = 0; i < _poolSize; i++)
        {
            Bullet bullet = Instantiate(_bulletPrefab, gameObject.transform);
            bullet.gameObject.SetActive(false);
            _pool[i] = bullet;
        }

        _subPool = new Bullet[_subPoolSize];
        for (int i = 0; i < _subPoolSize; i++)
        {
            Bullet subBullet = Instantiate(_subBulletPrefab, gameObject.transform);
            subBullet.gameObject.SetActive(false);
            _subPool[i] = subBullet;
        }
    }

    public Bullet ReturnBullet()
    {
        foreach (Bullet bullet in _pool)
        {
            if (bullet.gameObject.activeSelf == false)
            {
                bullet.gameObject.SetActive(true);
                bullet.OnSpawn();
                return bullet;
            }
        }
        return null;
    }

    public Bullet ReturnSubBullet()
    {
        foreach (Bullet bullet in _subPool)
        {
            if (bullet.gameObject.activeSelf == false)
            {
                bullet.gameObject.SetActive(true);
                bullet.OnSpawn();
                return bullet;
            }
        }
        return null;
    }
}