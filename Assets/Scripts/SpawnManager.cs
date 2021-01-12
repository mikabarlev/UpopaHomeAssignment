using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] Asteroid redAsteroidPrefab = null;
    [SerializeField] Asteroid blueAsteroidPrefab = null;
    [SerializeField] float spawnRate = 0.5f;
    const float spawnDelay = 0.5f;

    float xMax; 
    float xMin;
    float yMax;
    float yMin;

    public int redPercent = 20;   
    public int bluePercent = 70;   

    void Start()
    {
        UpdatePlayCoords();
        InvokeRepeating(nameof(AsteroidSpawn), spawnDelay, spawnRate);
    }

    void AsteroidSpawn()
    {
        int p = Random.Range(0, 100); //  p in [0,99] 
        if ((p -= bluePercent) < 0)  //  p in [0,b-1] -> b% blue asteroid
        {
            CreateAsteroid(blueAsteroidPrefab);
        }
        else if ((p -= redPercent) < 0)  // p + b in [b,r-1+b] -> p in [0,r-1] -> r% red asteroid
        {
            CreateAsteroid(redAsteroidPrefab);
        }
        else if (p < 0) // error ( r+b > 100 )
        {
            Debug.Log("bluePercent and redPercent add up to above 100%");
        }
        // else (100-r-b)% no asteroid 
    }

    void CreateAsteroid(Asteroid asteroidPreFab)
    {
        float xRand = Random.Range(xMin, xMax);
        float yRand = Random.Range(yMin, yMax);
        float orientationRand = Random.Range(0, 360);
        Instantiate(asteroidPreFab, new Vector3(xRand, yRand, 0), Quaternion.Euler(0, 0, orientationRand));
    }

    void UpdatePlayCoords()
    {
        Game game = (Game)FindObjectOfType(typeof(Game));
        xMax = game.GetPlayWidth() / 2;   
        xMin = xMax * (-1);   
        yMax = game.GetPlayHeight() / 2;    
        yMin = yMax * (-1);  
    }

}
