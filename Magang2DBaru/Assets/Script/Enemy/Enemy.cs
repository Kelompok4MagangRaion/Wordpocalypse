using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState //Mendefinisikan keadaan enemy
{
    idle, //Keadaan ketika enemynya diam
    walk, //Keadaan ketika enemynya jalan
    attack, //Keadaan ketika enemynya nyerang
    stagger //Keadaan ketika enemynya diserang
}

public class Enemy : MonoBehaviour
{
    [Header("State")]
    public EnemyState currentState;

    [Header("Enemy Stats")]
    public FloatValue enemyHealth;
    public string enemyName;
    public float health;
    public float damage;
    public float knock;
    public float moveSpeed;
    public Vector2 homePosition;

    [Header("Death")]
    public LootTable thisLoots;

    private void Awake()
    {
        health = enemyHealth.InitialValue;
        currentState = EnemyState.idle;
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            MakeLoot();
            this.gameObject.SetActive(false);
        }
    }

    void MakeLoot()
    {
        if (thisLoots != null)
        {
            Loot current = thisLoots.getLoot();
            if (current != null)
            {
                Instantiate(current.gameObject, transform.position, Quaternion.identity);
            }
        }
    }

    public void ChangeState(EnemyState changeState)
    {
        if(currentState != changeState)
        {
            currentState = changeState;
        }
    }

}
