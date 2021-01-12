using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderLeft : ColliderCyclic
{

    public override bool CollideFlag(CyclicMoveable cyclic)
    {
        return cyclic.collisionLeft;
    }


    // return cyclic pos (depands on screen size) 
    public override Vector3 CyclicPosition(Vector3 pos)
    {
        return new Vector3(pos.x + playWidth, pos.y, 0);
    }


    public override void TurnFlagOff(CyclicMoveable cyclic)
    {
        cyclic.collisionLeft = false;
    }


    // transform BoxCollider position and size to be "on" the screen edge
    public override void SetPositionAndSizeByScreenSize()
    {
        var component = GetComponent<BoxCollider>();
        component.size = new Vector3(0, playHeight, 1f);
        component.center = new Vector3(-xDistance, 0, 0);
    }


    // Set true at cyclic flag matching to this collider 
    // Set true at clone opposite flag (right - left) (top - bottom)
    public override void TurnFlagOn(CyclicMoveable cyclic, CyclicMoveable clone)
    {
        cyclic.collisionLeft = true;
        clone.collisionRight = true;
    }

}
