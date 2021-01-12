using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CyclicMoveable : MonoBehaviour
{

    public bool collisionRight = false;
    public bool collisionLeft = false;
    public bool collisionTop = false;
    public bool collisionBottom = false;

    public float speed = 2;
    public bool isShooting = false;
    public bool isShootingAndCyclic = false;


    public void Move()
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }


    public abstract void SelfDestruct(bool redChainReaction = true);

}
