using UnityEngine;

public class Asteroid : CyclicMoveable
{

    const int asteroidHealthDamage = 50;
    const int asteroidHitScore = 20;  
    [SerializeField] PickableItem pickableItemPrefab = null;
    [SerializeField] Game game = null;
    float astroidRadius = 0;
    const float explosionMultiplayer = 3;
    public bool visitedAndDestruct = false;


    void Start()
    {
        game = (Game)FindObjectOfType(typeof(Game));
        speed = 3f;
        var sc = GetComponent<SphereCollider>();
        if (sc)
        {
            astroidRadius = sc.radius * transform.localScale.x;
        }
        else
        {
            Debug.LogWarning("should be SphereCollider Component");
        }
    }


    void Update()
    {
        Move();
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Asteroid>())
        {
            SelfDestruct();  // Asteroid hit Asteroid: each one destroy itself
        }
        else if (other.gameObject.GetComponent<Bullet>())
        {
            game.AddScore(asteroidHitScore);  // Asteroid hit Bullet: add to game score
            Destroy(other.gameObject);    // Bullet should also destruct
            SelfDestruct();
        }
        else if (other.gameObject.GetComponent<SpaceShip>())
        {
            game.SubtractHealth(asteroidHealthDamage);  // Asteroid hit SpaceShip: subtract game health
            SelfDestruct();
        }
        
    }


    // Destruction of Asteroid
    // bool chainReaction : default value is true
    // bool chainReaction : false when this method get called by cyclic collider (asteroid got destroy by the reason it is ouside )
    // Blue Asteroid : chance to drop pickable item
    // Red Asteroid : check for other Red Asteroids in explosion radius 
    // visitedAndDestruct : used for Red Asteroids chain reaction
    public override void SelfDestruct(bool redChainReaction = true)
    {
        visitedAndDestruct = true;
        if (CompareTag("blue"))
        {
            DropPickableItem();
        }
        else if (CompareTag("red"))
        {
            if (redChainReaction)  
            {
                // redChainReaction == false : when ColliderCyclic destroy Red Asteroid for the reason it's outside the screen area
                // we dont want to start red explosion chain reaction 
                ExplosionRadius();  
            }
        }
        else
        {
            Debug.LogWarning("unexpected tag: "+tag);
        }
        Destroy(gameObject);
    }


    // Destroy Red Asteroids in radius of Explosion
    void ExplosionRadius()
    {
        Collider[] collidersInsideRadius = Physics.OverlapSphere(transform.position, astroidRadius * explosionMultiplayer);
        foreach (var hitCollider in collidersInsideRadius)
        {
            var hitAsteroid = hitCollider.gameObject.GetComponent<Asteroid>();
            if (hitAsteroid && hitAsteroid.CompareTag("red") && !hitAsteroid.visitedAndDestruct)
            {
                hitAsteroid.SelfDestruct();
            }
        }
    }

   
    void DropPickableItem()
    {
        if (Random.Range(0, 2) == 0) // 50% chance to drop an item  
        {
            Instantiate(pickableItemPrefab, transform.position, Quaternion.identity);
        }
    }
}
