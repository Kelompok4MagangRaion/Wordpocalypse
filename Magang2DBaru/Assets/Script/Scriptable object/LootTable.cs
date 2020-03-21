using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Looting
{
    public Loot thisLoot;
    public int chance;
}

[CreateAssetMenu]
public class LootTable : ScriptableObject
{
    public Looting[] loots;

    public Loot getLoot()
    {
        int cummulativeProb = 0;
        int currentProb = Random.Range(0, 100);

        for(int i = 0; i < loots.Length; i++)
        {
            cummulativeProb += loots[i].chance;
            if(currentProb <= cummulativeProb)
            {
                return loots[i].thisLoot;
            }
        }
        return null;
    }
}
