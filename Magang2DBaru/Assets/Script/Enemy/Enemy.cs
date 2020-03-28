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

    [Header("Death")]
    public LootTable thisLoots;
    public LootTable thisLoots2;

    [Header("Sound Effect")]
    public AudioSource audio;
    public AudioClip enemyWalk;
    public AudioClip enemyAttack;
    public AudioClip enemyAlive;
    public AudioClip enemyDeath;

    public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        health = enemyHealth.InitialValue;
        currentState = EnemyState.idle;
    }

    public void takeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            audio.PlayOneShot(enemyDeath);
            anim.SetTrigger("Death");
            StartCoroutine(DeathCo());
        }
    }

    IEnumerator DeathCo()
    {
        yield return new WaitForSeconds(1f);
        MakeLoot();
        MakeLoots();
        this.gameObject.SetActive(false);
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

    void MakeLoots()
    {
        if (thisLoots2 != null)
        {
            Loot current = thisLoots2.getLoot();
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
