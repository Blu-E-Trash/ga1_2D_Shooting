using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UI_UpgradeElement
{
    public string Name;

    public Button upgradeButton;
    public Text nameText;
    public Text valueText;
    public Text costText;

    [HideInInspector] public int level = 1;

    public void SetLevel(int newLevel)
    {
        level = newLevel;
    }
}

public class UI_UpgradeManager : MonoBehaviour
{
    [System.Serializable]
    public class UI_UpgradeElement
    {
        public Button upgradeButton;
        public Text nameText;
        public Text valueText;
        public Text costText;

        [HideInInspector] public int level = 1;
    }

    [Header("Upgrade UI Elements")]
    [SerializeField] private UI_UpgradeElement _damageUpgrade;
    [SerializeField] private UI_UpgradeElement _attackSpeedUpgrade;
    [SerializeField] private UI_UpgradeElement _moveSpeedUpgrade;

    [Header("Upgrade Values per Level")]
    [SerializeField] private float _damagePerLevel = 1f;
    [SerializeField] private float _fireRatePerLevel = 0.02f;
    [SerializeField] private float _moveSpeedPerLevel = 0.2f;

    [Header("Cost Settings")]
    [SerializeField] private int _baseCost = 5;
    [SerializeField] private int _costIncreasePerLevel = 5;

    private const string SAVE_KEY = "UpgradeData";

    private void Start()
    {
        Load();

        if (_damageUpgrade.upgradeButton != null)
            _damageUpgrade.upgradeButton.onClick.AddListener(OnClickDamageUpgrade);

        if (_attackSpeedUpgrade.upgradeButton != null)
            _attackSpeedUpgrade.upgradeButton.onClick.AddListener(OnClickAttackSpeedUpgrade);

        if (_moveSpeedUpgrade.upgradeButton != null)
            _moveSpeedUpgrade.upgradeButton.onClick.AddListener(OnClickMoveSpeedUpgrade);

        UpdateDamageUI();
        UpdateAttackSpeedUI();
        UpdateMoveSpeedUI();
    }

    // 공격력 업그레이드
    public void OnClickDamageUpgrade()
    {
        int cost = GetCurrentCost(_damageUpgrade.level);

        if (GameManager.Instance != null && GameManager.Instance.TrySpendMerit(cost))
        {
            if (PlayerStatus.Instance != null)
                PlayerStatus.Instance.UpgradeDamage(_damagePerLevel);

            _damageUpgrade.level++;
            UpdateDamageUI();

            Save();
        }
        else
        {
            Debug.Log("공훈(Merit)이 부족합니다!");
        }
    }

    private void UpdateDamageUI()
    {
        float currentBonus = (_damageUpgrade.level - 1) * _damagePerLevel;
        float nextBonus = currentBonus + _damagePerLevel;
        int cost = GetCurrentCost(_damageUpgrade.level);

        if (_damageUpgrade.nameText != null) _damageUpgrade.nameText.text = $"공격력 Lv.{_damageUpgrade.level}";
        if (_damageUpgrade.valueText != null) _damageUpgrade.valueText.text = $"+{currentBonus} -> +{nextBonus}";
        if (_damageUpgrade.costText != null) _damageUpgrade.costText.text = $"{cost} Pt";
    }

    // 공격속도 업그레이드
    public void OnClickAttackSpeedUpgrade()
    {
        int cost = GetCurrentCost(_attackSpeedUpgrade.level);

        if (GameManager.Instance != null && GameManager.Instance.TrySpendMerit(cost))
        {
            if (PlayerStatus.Instance != null)
                PlayerStatus.Instance.UpgradeFireRate(_fireRatePerLevel);

            _attackSpeedUpgrade.level++;
            UpdateAttackSpeedUI();

            Save();
        }
    }

    private void UpdateAttackSpeedUI()
    {
        if (_attackSpeedUpgrade.level >= 21)
        {
            _attackSpeedUpgrade.nameText.text = "공격속도 Lv.Max";
            _attackSpeedUpgrade.valueText.text = "현재 공격속도: 0.1";
            _attackSpeedUpgrade.costText.text = "최대 레벨입니다.";
            return;
        }

        float currentBonus = (_attackSpeedUpgrade.level - 1) * _fireRatePerLevel;
        float nextBonus = currentBonus + _fireRatePerLevel;
        int cost = GetCurrentCost(_attackSpeedUpgrade.level);

        if (_attackSpeedUpgrade.nameText != null) _attackSpeedUpgrade.nameText.text = $"공격속도 Lv.{_attackSpeedUpgrade.level}";
        if (_attackSpeedUpgrade.valueText != null) _attackSpeedUpgrade.valueText.text = $"+{currentBonus:F2} -> +{nextBonus:F2}";
        if (_attackSpeedUpgrade.costText != null) _attackSpeedUpgrade.costText.text = $"{cost} Pt";
    }

    // 이동속도 업그레이드
    public void OnClickMoveSpeedUpgrade()
    {
        int cost = GetCurrentCost(_moveSpeedUpgrade.level);

        if (GameManager.Instance != null && GameManager.Instance.TrySpendMerit(cost))
        {
            if (PlayerStatus.Instance != null)
                PlayerStatus.Instance.UpgradeMoveSpeed(_moveSpeedPerLevel);

            _moveSpeedUpgrade.level++;
            UpdateMoveSpeedUI();

            Save();
        }
    }

    private void UpdateMoveSpeedUI()
    {
        float currentBonus = (_moveSpeedUpgrade.level - 1) * _moveSpeedPerLevel;
        float nextBonus = currentBonus + _moveSpeedPerLevel;
        int cost = GetCurrentCost(_moveSpeedUpgrade.level);

        if (_moveSpeedUpgrade.nameText != null) _moveSpeedUpgrade.nameText.text = $"이동속도 Lv.{_moveSpeedUpgrade.level}";
        if (_moveSpeedUpgrade.valueText != null) _moveSpeedUpgrade.valueText.text = $"+{currentBonus:F1} -> +{nextBonus:F1}";
        if (_moveSpeedUpgrade.costText != null) _moveSpeedUpgrade.costText.text = $"{cost} Pt";
    }

    // 비용 계산 로직
    private int GetCurrentCost(int level)
    {
        return _baseCost + (level - 1) * _costIncreasePerLevel;
    }

    // 데이터 저장 및 불러오기 (JSON 방식)
    private void Save()
    {
        UpgradeSaveData data = new UpgradeSaveData(3);

        data.Name[0] = "Damage";
        data.Level[0] = _damageUpgrade.level;

        data.Name[1] = "AttackSpeed";
        data.Level[1] = _attackSpeedUpgrade.level;

        data.Name[2] = "MoveSpeed";
        data.Level[2] = _moveSpeedUpgrade.level;

        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(SAVE_KEY, jsonData);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        string jsonData = PlayerPrefs.GetString(SAVE_KEY, "");

        if (!string.IsNullOrEmpty(jsonData))
        {
            UpgradeSaveData data = JsonUtility.FromJson<UpgradeSaveData>(jsonData);

            if (data != null && data.Name != null && data.Level != null)
            {
                for (int i = 0; i < data.Name.Length; i++)
                {
                    if (data.Name[i] == "Damage") _damageUpgrade.level = data.Level[i];
                    else if (data.Name[i] == "AttackSpeed") _attackSpeedUpgrade.level = data.Level[i];
                    else if (data.Name[i] == "MoveSpeed") _moveSpeedUpgrade.level = data.Level[i];
                }
            }
        }

        if (PlayerStatus.Instance != null)
        {
            if (_damageUpgrade.level > 1)
                PlayerStatus.Instance.UpgradeDamage(_damagePerLevel * (_damageUpgrade.level - 1));

            if (_attackSpeedUpgrade.level > 1)
                PlayerStatus.Instance.UpgradeFireRate(_fireRatePerLevel * (_attackSpeedUpgrade.level - 1));

            if (_moveSpeedUpgrade.level > 1)
                PlayerStatus.Instance.UpgradeMoveSpeed(_moveSpeedPerLevel * (_moveSpeedUpgrade.level - 1));
        }
    }
}