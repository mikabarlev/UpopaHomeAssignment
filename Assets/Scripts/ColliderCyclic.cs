using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ColliderCyclic : MonoBehaviour
{
    
    [SerializeField] SpaceShip spaceshipPrefab = null;
    [SerializeField] Asteroid asteroidRedPrefab = null;
    [SerializeField] Asteroid asteroidBluePrefab = null;

    CyclicMoveable cyclicObj = null;
    PickableItem pickable = null;
    Bullet bullet = null;

    public float playWidth;
    public float playHeight;
    public float xDistance;
    public float yDistance;

    const bool disableRedChainReaction = false;

    public void Start()   
    {
        Game game = (Game)FindObjectOfType(typeof(Game));
        playWidth = game.GetPlayWidth();
        playHeight = game.GetPlayHeight();
        xDistance = playWidth / 2;  
        yDistance = playHeight / 2;  
        SetPositionAndSizeByScreenSize();   // transform colliders to be "on" screen edges
    }


    // OnTriggerExit: Asteroids, Bullets and PickableItems will be destroid.
    // SpaceShip will be destroid if it's ouside (means that there is another spaceship inside)
    // Spaceship will not be destroid if it's inside play area (SpaceShip can rotate while colliding)
    public void OnTriggerExit(Collider other)
    {
        cyclicObj = other.gameObject.GetComponent<CyclicMoveable>();
        if (cyclicObj)
        {
            HandleExitCyclic(cyclicObj);
            return;
        }
        bullet = other.gameObject.GetComponent<Bullet>();
        if (bullet && IsPosOutsideScreen(bullet.transform.position))
        {
            bullet.Selfdestruct();
            return;
        }
        pickable = other.gameObject.GetComponent<PickableItem>();
        if (pickable && IsPosOutsideScreen(pickable.transform.position))
        {
            pickable.Selfdestruct(); 
        }
    }


    // OnTriggerEnter: Moveable objects will be instantiated at the exact opposite position on the scrren (cyclic effect)
    public void OnTriggerEnter(Collider other)
    {
        cyclicObj = other.gameObject.GetComponent<CyclicMoveable>();  //  CyclicMoveable: spaceship or asteroid 
        if (cyclicObj && !CollideFlag(cyclicObj))   
        {
            // CollideFlag == false : when CyclicMoveable object moved thourds this collider and hit it 
            // CollideFlag == true : when CyclicMoveable object instantiated inside this collider (we want to ignore)
            Vector3 pos = CyclicPosition(cyclicObj.transform.position);
            HandleEnterCyclic(cyclicObj, pos);  
        }
    }


    // Instantiate new CyclicMoveable object at the exact opposite position to create the cyclic effect
    public void HandleEnterCyclic(CyclicMoveable cyclic, Vector3 pos)
    {
        CyclicMoveable clone;
        if (cyclic.GetComponent<SpaceShip>())
        {
            clone = Instantiate(spaceshipPrefab, pos, cyclic.transform.rotation);
            if (cyclic.isShooting)    
            {
                clone.isShootingAndCyclic = true;
                // this allows the "clone"(new) spaceship to continue shooting if "cyclic"(old) spaceShip is in shutting state when collide
                // if "space" is pressed
            }
        }
        else if (cyclic.CompareTag("blue"))
        {
            clone = Instantiate(asteroidBluePrefab, pos, cyclic.transform.rotation);
        }
        else if (cyclic.CompareTag("red"))
        {
            clone = Instantiate(asteroidRedPrefab, pos, cyclic.transform.rotation);
        }
        else
        {
            Debug.Log("unexpected tag: " + tag);
            return;
        }
        CopyFlags(cyclic, clone);   // clone needs cyclic flags to ignore coliisions with other colliders (corners)
        TurnFlagOn(cyclic, clone);  // cyclic needs to "ignore" this collider and clone needs to ignore the opposite collider
    }


    // Destroy CyclicMoveable object it it had left the play area
    public void HandleExitCyclic(CyclicMoveable cyclic)
    {
        TurnFlagOff(cyclic);
        if (IsFlagsFalse(cyclic))  // if cyclic object has all flags off (exit all colliders) 
        {
            if (IsPosOutsideScreen(cyclic.transform.position))
            // spaceship can rotate while colliding
            // so on exit it will still be inside the frame
            // for asteroids IsPosOutsideScreen will be true
            {
                cyclic.SelfDestruct(disableRedChainReaction); //  disableRedChainReaction = false;
                // when red asteroid destoy becouse its outdie screen are we dont need chain reaction explosion 
            }
        }
    }


    // return true if pos not inside play area
    public bool IsPosOutsideScreen(Vector3 pos)
    {
        return (Mathf.Abs(pos.x) > (playWidth / 2)) || (Mathf.Abs(pos.y) > (playHeight / 2));
    }


    // return true if all 4 flags false
    bool IsFlagsFalse(CyclicMoveable cyclic)
    {
        return !(cyclic.collisionRight || cyclic.collisionLeft || cyclic.collisionTop || cyclic.collisionBottom);
    }


    // set clone flags to match cyclic flags
    void CopyFlags(CyclicMoveable cyclic, CyclicMoveable clone)
    {
        clone.collisionRight = cyclic.collisionRight;
        clone.collisionLeft = cyclic.collisionLeft;
        clone.collisionTop = cyclic.collisionTop;
        clone.collisionBottom = cyclic.collisionBottom;
    }


    // Set false at cyclic flag matching to the collider  (Right / Left / Top / Bottom)
    public abstract void TurnFlagOff(CyclicMoveable cyclic);


    // return collide flag (collisionRight / collisionLeft / collisionTop / collisionBottom)
    public abstract bool CollideFlag(CyclicMoveable cyclic);


    // Set true at cyclic flag matching to this collider 
    // Set true at clone opposite flag (right - left) (top - bottom)
    public abstract void TurnFlagOn(CyclicMoveable cyclic, CyclicMoveable clone);


    // return cyclic pos (depands on screen size) 
    public abstract Vector3 CyclicPosition(Vector3 pos);


    // transform BoxCollider position and size to be "on" the screen edge
    public abstract void SetPositionAndSizeByScreenSize();


}
