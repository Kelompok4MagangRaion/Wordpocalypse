using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KontrolerDasar : MonoBehaviour
{
    [Header("Player Stuff")]
    public FloatValue playerHealth;
    public float health;
    public Vector2 change;
    public float speed;
    public float jump = 150;
    public bool canAttack = true;
    public LayerMask ground;

    [Header("Player Attack")]
    public float attackDmg;
    public float shootDmg;
    public float knock;
    public Transform AttackPoint;
    public float AttackRange = 0.5f;
    public LayerMask enemyLayers;
    public GameObject bulletHole;
    public GameObject projectile;

    [Header("Stuff")]
    private float tempX;
    private float tempY;
    [SerializeField]private Rigidbody2D rbd;
    private Animator anim;
    private SpriteRenderer sprite;

    private void Start()
    {
        health = playerHealth.InitialValue;
        tempX = -1;
        tempY = 0;
        rbd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        isGround();
        Jump();
        change = Vector2.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        Attack();
    }

    void FixedUpdate()
    {
        UpdateAnimationMove();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Trap")
        {
            Destroy(this.gameObject);
        }
    }

    bool isGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, ground);
        /*Debug.DrawRay(transform.position, Vector2.down * 1f, Color.red);*/ // Untuk menngetahui seberapa panjang RaycastHit2D
        if (hit)
        {
            anim.SetBool("isJumping", false);
            return true;
        }
        else
        {
            return false;
        }
    }

    void Move()
    {
        if (isGround() && canAttack)
        {
            rbd.velocity = change * speed;
            if (change.x > 0)
            {
                //sprite.flipX = false;
                transform.localScale = new Vector3(1, 1, 1);
            }
            if (change.x < 0)
            {
                //sprite.flipX = true;
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    void UpdateAnimationMove()
    {
        if(change != Vector2.zero)
        {
            Move();
            tempX = change.x;
            tempY = change.y;
            anim.SetBool("Moving",true);
        }
        else
        {
            anim.SetBool("Moving", false);
        }
    }

    void Jump()
    {
        if (isGround())
        {
            if (Input.GetButtonDown("Jump"))
            {
                rbd.AddForce(new Vector2(0, 1f) * jump);
                anim.SetBool("isJumping", true);
            }
        }
    }

    void Attack()
    {
        if (canAttack)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                Melee();
            }
            if (Input.GetMouseButton(0))
            {
                Shooting();
            }
        }
    }

    void Melee()
    {
        canAttack = false;
        anim.SetTrigger("Attack");

        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemy)
        {
            Rigidbody2D hit = enemy.GetComponent<Rigidbody2D>();
            //Untuk Knockback
            if(hit != null) 
            {
                Vector2 forceDirection = hit.transform.position - transform.position;
                Vector2 force = forceDirection.normalized * knock;
                hit.velocity = force;
            }
            enemy.GetComponent<Enemy>().takeDamage(attackDmg);
        }
        StartCoroutine(AttackCo());
        StopCoroutine(AttackCo());
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }

    void Shooting()
    {
        canAttack = false;
        anim.SetTrigger("Shooting");
        Vector2 temp = new Vector2(tempX, tempY);
        Bullet bullet = Instantiate(projectile, bulletHole.transform.position, Quaternion.identity).GetComponent<Bullet>();
        bullet.setup(temp, ChangeDirectionBullet());
        StartCoroutine(AttackCo());
        StopCoroutine(AttackCo());
    }

    Vector3 ChangeDirectionBullet()
    {
        float temp = Mathf.Atan2(tempY, tempX) * Mathf.Rad2Deg;
        return new Vector3(0, 0, temp);
    }

    IEnumerator AttackCo()
    {
        yield return new WaitForSeconds(0.7f);
        canAttack = true;
    }

    public void takeDamage(float dmg)
    {
        health -= dmg;
        if(health <= 0)
        {
            this.gameObject.SetActive(false);
        }
    }

}
