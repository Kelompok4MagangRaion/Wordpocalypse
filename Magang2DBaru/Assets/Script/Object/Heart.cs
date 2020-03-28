using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour
{

    public void Use(float ammountToIncrease)
    {
        if(KontrolerDasar.health <= 10f)
        {
            KontrolerDasar.health += ammountToIncrease;
        }
        else if(KontrolerDasar.health > 10)
        {
            KontrolerDasar.health = 10;
        }
    }
}
