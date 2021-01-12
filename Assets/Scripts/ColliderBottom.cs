using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderBottom : ColliderCyclic
{

    public override bool CollideFlag(CyclicMoveable cyclic)
    {
        return cyclic.collisionBottom;
    }


    // return cyclic pos (depands on screen size)
    public override Vector3 CyclicPosition(Vector3 pos)
    {
        return new Vector3(pos.x, pos.y + playHeight, 0);
    }


    public override void TurnFlagOff(CyclicMoveable cyclic)
    {
        cyclic.collisionBottom = false;
    }


    // transform BoxCollider position and size to be "on" the screen edge
    public override void SetPositionAndSizeByScreenSize()
    {
        var component = GetComponent<BoxCollider>();
        component.size = new Vector3(playWidth, 0, 1f);
        component.center = new Vector3(0, -yDistance, 0);
    }


    // Set true at cyclic flag matching to this collider 
    // Set true at clone opposite flag (right - left) (top - bottom)
    public override void TurnFlagOn(CyclicMoveable cyclic, CyclicMoveable clone)
    {
        cyclic.collisionBottom = true;
        clone.collisionTop = true;
    }

}
