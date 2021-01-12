using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : MonoBehaviour
{

    [SerializeField] int[] values = null;
    [SerializeField] Sprite[] pointsSprites = null; 
    const float speed = 2f;
    int random = 0;
    int value = 0;
    bool isFuel = false;


    void Start()
    {
        SetItemParams(); // value , sprite , fuel vs points
    }


    void Update()
    {
        Move();
    }


    // Item value selected randomly from values array.
    // Set the matching 'number icon' to the object sprite.
    // selected randomly: fuel or points. (can't be both)
    // sprite will be white color for points item , and healthbar color for fuel item.
    void SetItemParams()
    { 
        int n = values.Length;
        random = Random.Range(0, n);      
        value = values[random];            // item value

        SpriteRenderer spriteComp = GetComponent<SpriteRenderer>();
        if (n != pointsSprites.Length)
        {
            Debug.Log("SerializeFields values and pointsSprites should be the same length");
            random = 0;
        }
        spriteComp.sprite = pointsSprites[random];    //  item sprite match to item value

        isFuel = (Random.Range(0, 2) != 0);   // 50% chance to be fuel and 50% to be points
        if (isFuel)    
        {
            // if isFuel sprite color will take current healthbar color. if it's points items color will be white
            HealthBar healthBar = (HealthBar)FindObjectOfType(typeof(HealthBar));
            spriteComp.color = healthBar.GetHealthColor();
        }
    }


    // if item is fuel: fuel is value
    // if item is points: fuel is 0
    // fuel will add to health and points will add to score
    public int GetFuel()  
    {
        return isFuel ? value : 0;
    }


    // if item is points: points is value.
    // if item is fuel: points is 0.
    // points will add to score and fuel will add to health
    public int GetPoints()
    {
        return isFuel ? 0 : value;
    }


    public void Selfdestruct()
    {
        Destroy(gameObject);
    }


    public void Move()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);
    }

}
