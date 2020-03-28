using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Zombie
{
    public override void changeDirection(Vector2 distance)
    {
        distance = distance.normalized;
        if (distance.x > 0)
        {
            transform.localScale = new Vector3(4, 4, 1);
        }
        if (distance.x < 0)
        {
            transform.localScale = new Vector3(-4, 4, 1);
        }
    }
}
