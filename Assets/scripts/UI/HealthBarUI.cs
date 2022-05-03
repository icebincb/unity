using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefeb;
    public Transform barPoint;
    public bool alwaysVisible;
    public float visibleTime;
    public float timelife;
    Image healthSlider;
    Transform UIBar;
    Transform cam;
    CharacterStats currentStats;
    private void Awake()
    {
        currentStats = GetComponent<CharacterStats>();
        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }
    private void OnEnable()
    {
        cam = Camera.main.transform;
        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIBar= Instantiate(healthUIPrefeb, canvas.transform).transform;
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();
                UIBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }
    private void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (UIBar != null){
        if (currentHealth <= 0)
        {
            Destroy(UIBar.gameObject);
        }
        UIBar.gameObject.SetActive(true);
        timelife = visibleTime;
        float siliderPercent = currentHealth / maxHealth;
        healthSlider.fillAmount = siliderPercent;
        }
    }

    private void LateUpdate()
    {
        if (UIBar != null)
        {
            UIBar.position = barPoint.position;
            UIBar.forward = -cam.forward;
            if(timelife<=0 && !alwaysVisible)
            {
                UIBar.gameObject.SetActive(false);
            }
            else
            {
                timelife -= Time.deltaTime;
            }
        }
    }
}
