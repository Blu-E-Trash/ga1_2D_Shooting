using System.Collections;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus Instance { get; private set; }

    [Header("Player States")]
    public bool IsAutoMode = false;

    [Header("Permanent Upgrades (영구 스탯)")]
    private float _bonusDamage = 0f;
    private float _baseFireRate = 0.5f;
    private float _baseMoveSpeed = 5f;

    [Header("Temporary Buffs (임시 스탯)")]
    private float _tempDamage = 0f;
    private float _tempFireRate = 0f;
    private float _tempMoveSpeed = 0f;

    public float FinalBonusDamage => _bonusDamage + _tempDamage;

    public float FinalFireRate => Mathf.Max(0.1f, _baseFireRate - _tempFireRate);

    public float FinalMoveSpeed => _baseMoveSpeed + _tempMoveSpeed;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void ToggleAutoMode()
    {
        IsAutoMode = !IsAutoMode;
    }

    public void UpgradeDamage(float amount)
    {
        _bonusDamage += amount;
    }

    public void UpgradeFireRate(float amount)
    {
        _baseFireRate -= amount;
        if (_baseFireRate < 0.1f) _baseFireRate = 0.1f;
    }

    public void UpgradeMoveSpeed(float amount)
    {
        _baseMoveSpeed += amount;
    }

    public void ApplyTempDamageBuff(float amount, float duration = 10f)
    {
        StartCoroutine(DamageBuffRoutine(amount, duration));
    }

    public void ApplyTempFireRateBuff(float amount, float duration = 10f)
    {
        StartCoroutine(FireRateBuffRoutine(amount, duration));
    }

    public void ApplyTempMoveSpeedBuff(float amount, float duration = 10f)
    {
        StartCoroutine(MoveSpeedBuffRoutine(amount, duration));
    }

    private IEnumerator DamageBuffRoutine(float amount, float duration)
    {
        _tempDamage += amount; // 버프 적용
        yield return new WaitForSeconds(duration);
        _tempDamage -= amount; // 버프 해제
    }

    private IEnumerator FireRateBuffRoutine(float amount, float duration)
    {
        _tempFireRate += amount;
        yield return new WaitForSeconds(duration);
        _tempFireRate -= amount;
    }

    private IEnumerator MoveSpeedBuffRoutine(float amount, float duration)
    {
        _tempMoveSpeed += amount;
        yield return new WaitForSeconds(duration);
        _tempMoveSpeed -= amount;
    }
}