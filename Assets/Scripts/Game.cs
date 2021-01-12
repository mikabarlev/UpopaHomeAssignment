using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public SpaceShip spaceShipPrefab = null;
    public HealthBar healthBar = null;
    public Text textComponentScore = null;

    const float repeatRateHealth = 1f;
    const float repeatRateDamage = 5f;

    const int maxHealth = 1000;
    const int healthDamageIncrease = 5;
    int currentHealthDamage = 50;
    int currentScore = 0;  // initialize to 0

    float playWidth;
    float playHeight;

    Canvas canvas;
    GameObject uiGameOver;
    GameObject uiLeft;
    GameObject uiRight;

    bool freezeGame = false;
    const float raySize = 100f;
   
    void Start()
    {
        canvas = GetComponentInChildren<Canvas>();
        uiGameOver = canvas.transform.GetChild(2).gameObject;
        uiGameOver.SetActive(false);    // hides 'GAME OVER' text

        SetPlaySizeByCanvas();   

        SetScore(currentScore);  
        healthBar.SetMax(maxHealth); 

        // Health over time:
        InvokeRepeating(nameof(UpdateDamageOverTime), repeatRateDamage, repeatRateDamage);  
        InvokeRepeating(nameof(SubtractHealthOverTime), repeatRateHealth, repeatRateHealth); 

        // Instantiate SpaceShip at the center of the play space with random orientation:
        Instantiate(spaceShipPrefab, Vector3.zero, Quaternion.Euler(0, 0, Random.Range(0, 360)));  

    }

    void Update()
    {
        CheckClickOnPickable();
    }


    // Set playHeight & playWidth values by canvas size
    void SetPlaySizeByCanvas()
    {
        canvas = GetComponentInChildren<Canvas>();
        uiLeft = canvas.transform.GetChild(0).gameObject;
        uiRight = canvas.transform.GetChild(1).gameObject;
        float w1 = uiLeft.GetComponent<RectTransform>().rect.width;  // Width of the UI grey rectangle
        float w2 = uiRight.GetComponent<RectTransform>().rect.width;   // // Width of UI the grey rectangle
        playHeight = Camera.main.orthographicSize * 2;  
        float newWidth = Screen.width - w1 - w2; 
        playWidth = newWidth / (Screen.height / playHeight);
    }

    // If the player clicks on pickable item
    private void CheckClickOnPickable()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, raySize))
            {
                var pickable = hit.transform.GetComponent<PickableItem>(); 
                if (pickable && !freezeGame) // freezeGame is state when GameOver UI appears 
                {
                    AddHealth(pickable.GetFuel());  // if pickableItem is fuel adds value, else 0
                    AddScore(pickable.GetPoints());  // if pickableItem is score adds value, else 0
                    pickable.Selfdestruct();
                }
            }
        }
    }

    // load Game scene
    // restart buttom in 'gameOverui' is attached to this method
    public void LoadGameScene() 
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(0);
    }


    // this methos is called when health reaches 0.
    public void EndGame()
    {
        uiGameOver.SetActive(true); // show game over ui (with restart button)
        freezeGame = true;  // flag used to disable clicking on pickable items (while game over)
        Time.timeScale = 0f; // freeze frame 
    }


    public void AddScore(int s)
    {
        currentScore += s;
        textComponentScore.text = currentScore.ToString();
    }

    void SetScore(int s)
    {
        currentScore = s;
        textComponentScore.text = currentScore.ToString();
    }

    void AddHealth(int h)
    {
        healthBar.Add(h);
    }

    public void SubtractHealth(int h)
    {
        healthBar.Subtract(h);
    }

    void SubtractHealthOverTime() 
    {
        SubtractHealth(currentHealthDamage);
    }

    void UpdateDamageOverTime()
    {
        currentHealthDamage += healthDamageIncrease;
    }

    public float GetPlayWidth()
    {
        return playWidth;
    }

    public float GetPlayHeight()
    {
        return playHeight;
    }

    

}
