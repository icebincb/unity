using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthUI : MonoBehaviour
{
    Text levelText;
    Image healthSlider;
    Image expSlider;
    private void Awake()
    {
        levelText = transform.GetChild(2).GetComponent<Text>();
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
    }
    private void Update()
    {
        levelText.text = "Level: " + Gamemanager.Instance.playerstats.charactorData.currentLevel.ToString("00");
        UpdateHealth();
        UpdateEXP();
    }
    void UpdateHealth()
    {
        float sliderPercent = Gamemanager.Instance.playerstats.CurrentHealth / Gamemanager.Instance.playerstats.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
    void UpdateEXP()
    {
        float sliderPercent =(float) Gamemanager.Instance.playerstats.Current_EXP / Gamemanager.Instance.playerstats.Base_EXP;
        //Debug.Log(sliderPercent);
        expSlider.fillAmount = sliderPercent;
    }
}
