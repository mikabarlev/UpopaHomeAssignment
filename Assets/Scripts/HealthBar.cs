using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider healthSlider;
    public Gradient gradient;
    public Image fill;
    public Game game;

    private void Start()
    {
        game = (Game)FindObjectOfType(typeof(Game));
    }

    public void Add(int health)
    {
        healthSlider.value += health;
        fill.color = GetHealthColor();  
    }

    public void Subtract(int health)
    {
        healthSlider.value -= health;

        if (healthSlider.value <= 0)
        {
            game.EndGame();
        }
        fill.color = GetHealthColor();
    }

    public void SetMax(int health)
    {
        healthSlider.maxValue = health;
        healthSlider.value = health;
        fill.color = GetHealthColor();
    }

    public int Get()
    {
        return (int)healthSlider.value;  
    }

    public Color GetHealthColor()
    {
        return gradient.Evaluate(healthSlider.normalizedValue);
    }

}
