using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : CyclicMoveable
{
    // rotation
    const int rotateRight = -1;
    const int rotateLeft = 1;
    const float dragDuration = 1.5f;
    public float rotationSpeed = 80;

    float rotation = 0;
    float dragTimer = 0f;
    bool rotationHasStarted = false;

    // shooting
    [SerializeField] Bullet bulletPrefab = null;
    Bullet bullet;
    public float shootingRate = 0.2f;
    public float bulletShift = 0.3f;


    void Start()
    {
        speed = 2f;
    }


    void Update()
    {
        RotateAndDrag();
        Move();
        Fire();
    }


    public void RotateAndDrag()
    {
        dragTimer += Time.deltaTime;
        if (Input.GetKey(KeyCode.D))  // rotation right
        {
            rotation = rotateRight; 
            Rotate(rotation); 
            dragTimer = 0;    
            rotationHasStarted = true;
        }
        if (Input.GetKey(KeyCode.A))  // rotation left
        {
            rotation = rotateLeft;
            Rotate(rotation);
            dragTimer = 0;
            rotationHasStarted = true;
        }
        else if (dragTimer < dragDuration && dragTimer > 0 && rotationHasStarted == true)  // drag comtinues for dragDuration time
        {
            // rotationHasStarted is false only at the beggining of the game
            Rotate(rotation * DragForce(dragTimer));   // DragForce decrease from 1 to 0 
        }
    }


    public void Rotate(float degree)
    {
        transform.Rotate(Vector3.forward, rotationSpeed * degree * Time.deltaTime);
    }


    public float DragForce(float dragTimer)
    {
        //  dragTimer : sum of Time.deltaTime when draging
        //  dragDuration : 1.5 sec 
        //  0 < return value < 1 
        //  decresing with rate f(x) = x^2 
        return Mathf.Pow((dragDuration - dragTimer) / dragDuration, 2);
    }


    // Shoots bullets countinuesly while pressing "space"
    private void Fire()
    {

        if (Input.GetKeyDown(KeyCode.Space))  
        {
            isShooting = true;
            InvokeRepeating(nameof(ShootBullet), 0, shootingRate);  // constant shooting every shootingRate
        }
        else if (Input.GetKey(KeyCode.Space) && isShootingAndCyclic)  // allows constant shooting right after cyclic (for the "new" SpaceShip)
        {   
            InvokeRepeating(nameof(ShootBullet), 0, shootingRate);
            isShootingAndCyclic = false;
            isShooting = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))  
        {
            isShooting = false; 
            CancelInvoke(nameof(ShootBullet));  // stop constant shooting
        }
    }


    public void ShootBullet()
    {
        bullet = Instantiate(bulletPrefab, transform.position , transform.rotation) as Bullet;
        bullet.MoveStep(bulletShift); // position the bullet right infront of the spaceship head
    }


    public override void SelfDestruct(bool redChainReaction = true)
    {
        Destroy(gameObject);
    }

}
