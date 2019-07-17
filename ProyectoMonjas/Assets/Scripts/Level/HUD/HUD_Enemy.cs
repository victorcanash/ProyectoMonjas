using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD_Enemy : MonoBehaviour
{
    public Enemy enemy { get; set; }
    [SerializeField] private Slider slider = null;

    public void Init(Enemy linkedEnemy, float maxHealthEnemy, float currentHealth)
    {
        enemy = linkedEnemy;
        slider.maxValue = maxHealthEnemy;
        slider.minValue = 0;
        slider.value = currentHealth;
    }

    private void Update()
    {
        transform.position = new Vector3(enemy.transform.position.x, enemy.transform.position.y + 2.5f, enemy.transform.position.z);
        transform.rotation = GameManager.Instance.LevelController.gameCamera.transform.rotation;
    }

    public void UpdateHealthSlider(float currentHealth)
    {
        slider.value = currentHealth;
    }
}
