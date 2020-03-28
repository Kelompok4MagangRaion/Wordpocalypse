using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Enemy
{
    private Rigidbody2D rb;

    [Header ("Enemy Chase")]
    public Transform target;
    public float chaseRadius;
    public float attackRadius;
    public Collider2D boundary;

    [Header ("Enemy Attack")]
    public Transform AttackPoint;
    public float AttackRange = 0.5f;
    public LayerMask playerLayer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate()
    {
        checkDistance();
    }

    public virtual void checkDistance()
    {
        if(Vector2.Distance(target.position, transform.position) <= chaseRadius 
            && Vector2.Distance(target.position, transform.position) > attackRadius
            && boundary.bounds.Contains(target.transform.position))
        {
            if (currentState == EnemyState.idle || currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                audio.PlayOneShot(enemyWalk);
                Vector2 temp =  Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                temp.y = transform.position.y;
                changeDirection(temp - new Vector2(transform.position.x, transform.position.y));
                rb.MovePosition(temp);
                ChangeState(EnemyState.walk);
                anim.SetBool("Walking", true);
            }
        }
        else if(Vector2.Distance(target.position, transform.position) > chaseRadius
            || !boundary.bounds.Contains(target.transform.position))
        {
            anim.SetBool("Walking", false);
        }
        else if(Vector2.Distance(target.position, transform.position) <= chaseRadius
            && Vector2.Distance(target.position, transform.position) <= attackRadius)
        {
            if(currentState == EnemyState.walk && currentState != EnemyState.stagger)
            {
                Attack();
                StartCoroutine(AttackCo());
            }
        }
    }

    public void Attack()
    {
        audio.PlayOneShot(enemyAttack);
        Collider2D[] hitPlayer = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, playerLayer);

        foreach (Collider2D player in hitPlayer)
        {
            Rigidbody2D hit = player.GetComponent<Rigidbody2D>();
            //Untuk Knockback
            if (hit != null)
            {
                Vector2 forceDirection = hit.transform.position - transform.position;
                Vector2 force = forceDirection.normalized * knock;
                hit.velocity = force;
            }
            player.GetComponent<KontrolerDasar>().takeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }

    IEnumerator AttackCo()
    {
        currentState = EnemyState.attack;
        anim.SetBool("Attack", true);
        yield return new WaitForSeconds(1f);
        currentState = EnemyState.idle;
        anim.SetBool("Attack", false);

    }

    public virtual void changeDirection(Vector2 distance)
    {
        distance = distance.normalized;
        if(distance.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        if(distance.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    
    
}
