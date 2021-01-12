using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float speed;
    float lifespan = 5f;


    void Start()
    {
        speed = 5f;
        Destroy(gameObject, lifespan);
        // lifespan > screensize/speed
        // Bullets that instantiated ouside the screen (will mot be destroid by cyclic colllider) so they will be destroid evantually by time
        // That can happend when SpaceShip shoot bullet when its half ouside the screen and facing ouside
    }


    void Update()
    {
        Move();
    }


    public void Move() 
    {
        transform.Translate(Vector3.right * Time.deltaTime * speed);
    }


    public void MoveStep(float size)
    {
        transform.Translate(Vector3.right * size);
    }


    public void Selfdestruct()
    {
        Destroy(gameObject);
    }

}
