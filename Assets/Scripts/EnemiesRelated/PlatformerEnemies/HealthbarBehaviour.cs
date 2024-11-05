using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarBehaviour : MonoBehaviour
{
    private Transform healthBar;
    public Slider slider;
    public Color low;
    public Color high;

    private void Awake()
    {
        healthBar = GetComponent<Transform>();
    }
    public void SetHealth(float health, float maxHealth)
    {
        slider.gameObject.SetActive(health < maxHealth);
        slider.value = health;
        slider.maxValue = maxHealth;
        slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(low, high, slider.normalizedValue);
    }

    private void Update()
    {
        healthBar.rotation = Quaternion.identity;
    }

}
