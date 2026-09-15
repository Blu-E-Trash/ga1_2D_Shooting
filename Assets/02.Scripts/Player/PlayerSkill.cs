using UnityEngine;

public class PlayerSkill : MonoBehaviour
{
    [SerializeField]
    private GameObject _targetPosition;
    [SerializeField]
    private GameObject _boom;

    private float _coolTime = 10f;
    private float _currentCoolTime = 0f;
    private bool _isCoolTime = false;

    private void Awake()
    {
        _targetPosition = GameObject.FindGameObjectWithTag("Target");
    }

    private void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver)
        {
            return;
        }

        if (SimpleInput.GetKeyDown(KeyCode.B))
        {
            UseSkill();
        }

        if (_isCoolTime)
        {
            _currentCoolTime += Time.deltaTime;

            if (_currentCoolTime >= _coolTime)
            {
                _isCoolTime = false;
                _currentCoolTime = 0f;
            }
        }
    }

    public void UseSkill()
    {
        if (!_isCoolTime)
        {
            string boomTag = _boom.name;
            ObjectManager.Instance.SpawnFromPool(boomTag, _targetPosition.transform.position, Quaternion.identity);

            _isCoolTime = true;
            _currentCoolTime = 0f;
        }
    }
}