using UnityEngine;
using UnityEngine.UI;

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

    private void Start()
    {
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

        // 생존 시간 대신 현재 공훈(CurrentMerit)이 충분한지 검사합니다.
        if (GameManager.Instance != null && GameManager.Instance.TrySpendMerit(cost))
        {
            GameManager.Instance.CurrentMerit -= cost; // 공훈 차감

            if (PlayerStatus.Instance != null)
                PlayerStatus.Instance.UpgradeDamage(_damagePerLevel);

            _damageUpgrade.level++;
            UpdateDamageUI();
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
            GameManager.Instance.CurrentMerit -= cost;

            if (PlayerStatus.Instance != null)
                PlayerStatus.Instance.UpgradeFireRate(_fireRatePerLevel);

            _attackSpeedUpgrade.level++;
            UpdateAttackSpeedUI();
        }
    }

    private void UpdateAttackSpeedUI()
    {
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
            GameManager.Instance.CurrentMerit -= cost;

            if (PlayerStatus.Instance != null)
                PlayerStatus.Instance.UpgradeMoveSpeed(_moveSpeedPerLevel);

            _moveSpeedUpgrade.level++;
            UpdateMoveSpeedUI();
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
}