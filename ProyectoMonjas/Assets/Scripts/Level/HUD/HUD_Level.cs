using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Level : MonoBehaviour
{
    public DynamicJoystick moveJoystick = null, attackJoystick = null;
    [SerializeField] private Slider health_slider = null;
    [SerializeField] private TextMeshProUGUI txt_round = null, txt_kills = null;

    public void Init()
    {
        UpdateTextRound(1, true);
        UpdateTextKills(0);
    }

    public void InitPlayerHealthSlider(float maxHealth)
    {
        health_slider.maxValue = maxHealth;
        health_slider.value = maxHealth;
        health_slider.minValue = 0;
    }

    public void UpdateHealthSlider(float currentHealth)
    {
        health_slider.value = currentHealth;
    }

    public void DisappearTextRound()
    {
        txt_round.gameObject.SetActive(false);
    }

    public void AppearTextRound()
    {
        txt_round.gameObject.SetActive(true);
    }

    public void UpdateTextRound(int round, bool active)
    {
        txt_round.gameObject.SetActive(active);
        txt_round.text = "ROUND " + round.ToString();
    }

    public void UpdateTextKills(int kills)
    {
        txt_kills.text = "KILLS: " + kills.ToString();
    }
}
