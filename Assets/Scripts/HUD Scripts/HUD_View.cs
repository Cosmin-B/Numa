using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_View : MonoBehaviour
{
    private Image _energyFiller;
    private Image _fuelFiller;
    private HUD_Model _hudModel;

    private const float _minimumAcceptedFloatValue = 0.00001f;

    private void Start()
    {
        _energyFiller = GameObject.Find("EnergyFiller").GetComponent<Image>();
        Debug.Assert(_energyFiller != null);

        _fuelFiller = GameObject.Find("FuelFiller").GetComponent<Image>();
        Debug.Assert(_fuelFiller != null);

        _hudModel = GetComponent<HUD_Model>();
        Debug.Assert(_hudModel != null);

        _energyFiller.fillAmount = _hudModel.currentEnergy / _hudModel.maxEnergy;
        _fuelFiller.fillAmount = _hudModel.currentFuel / _hudModel.maxFuel;

        _hudModel.HudUpdate += UpdateHUD;
    }

    void UpdateHUD(HUD_Model.PowerTypes powerType, float targetValue, bool stopCoroutine = false)
    {
        if (stopCoroutine)
        {
            StopAllCoroutines();

            /// that means the player died.
            _energyFiller.fillAmount = 0;
        }
        else
            StartCoroutine(LerpHUDValues(powerType, targetValue));
    }

    IEnumerator LerpHUDValues(HUD_Model.PowerTypes powerType, float targetValue)
    {
        bool lerpDone = false;

        float maxValueBasedOnType = powerType == HUD_Model.PowerTypes.POWER_ENERGY ? _hudModel.maxEnergy : _hudModel.maxFuel;
        float endValue = targetValue / maxValueBasedOnType;

        while(!lerpDone)
        {
            if (powerType == HUD_Model.PowerTypes.POWER_ENERGY)
                _energyFiller.fillAmount = Mathf.Lerp(_energyFiller.fillAmount, endValue, Time.deltaTime  * _hudModel.updateHudSmoothMultiplier);
            else
                _fuelFiller.fillAmount = Mathf.Lerp(_fuelFiller.fillAmount, endValue, Time.deltaTime * _hudModel.updateHudSmoothMultiplier);

            float typeValue = powerType == HUD_Model.PowerTypes.POWER_ENERGY ? _energyFiller.fillAmount : _fuelFiller.fillAmount;

            if (nearlyEqual(typeValue, endValue, _minimumAcceptedFloatValue))
                lerpDone = true;

            yield return null;
        }
    }

    public static bool nearlyEqual(float a, float b, float epsilon)
    {
        float absA = Mathf.Abs(a);
        float absB = Mathf.Abs(b);
        float diff = Mathf.Abs(a - b);

        if (a == b)
        { // shortcut, handles infinities
            return true;
        }
        else if (a == 0 || b == 0 || diff < float.MinValue)
        {
            // a or b is zero or both are extremely close to it
            // relative error is less meaningful here
            return diff < (epsilon * float.MinValue);
        }
        else
        { // use relative error
            return diff / (absA + absB) < epsilon;
        }
    }
}
