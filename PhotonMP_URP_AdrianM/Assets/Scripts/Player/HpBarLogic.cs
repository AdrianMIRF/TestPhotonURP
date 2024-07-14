using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpBarLogic : MonoBehaviour
{
    [SerializeField] private Image currentHP_Ref = null;
    [SerializeField] private Image hpDiff_Ref = null;

    private PlayerHealth _playerHealth;

    private bool _setupDone = false;
    private float _smoothTime = 0.3f;
    private float _yVelocity = 0.0f;

    public void Setup(PlayerHealth playerHealth)
    {
        _playerHealth = playerHealth;
        _setupDone = true;
    }

    void LateUpdate()
    {
        if (!_setupDone)
        {
            return;
        }

        currentHP_Ref.fillAmount = _playerHealth.GetCurrentHealth() / _playerHealth.GetMaxHealth();

        if (hpDiff_Ref.fillAmount != currentHP_Ref.fillAmount)
        {
            hpDiff_Ref.fillAmount = Mathf.SmoothDamp(hpDiff_Ref.fillAmount, currentHP_Ref.fillAmount, ref _yVelocity, _smoothTime);
        }
    }
}