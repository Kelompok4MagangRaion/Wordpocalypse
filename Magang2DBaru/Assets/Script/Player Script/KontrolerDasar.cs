using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KontrolerDasar : MonoBehaviour
{
    [Header("Player Stuff")]
    public FloatValue playerHealth;
    public static float health;
    public Vector2 change;
    public float speed;
    public float jump = 150;
    public bool canAttack = true;
    public LayerMask ground;
    public HealthBar healthBar;

    [Header("Player Inventory")]
    public PlayerInventory Inventory;

    [Header("Player Weapon & Durability")]
    public InventoryItem Sword;
    public InventoryItem Gun;
    public FloatValue durabiltySword;
    public FloatValue bullets;
    public GameObject durSword;
    public GameObject bullText;

    [Header("Player Attack")]
    public float attackDmg;
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

    [Header("Sound Effect")]
    AudioSource audio;
    public AudioClip playerWalk;
    public AudioClip playerAttack;
    public AudioClip playerShoot;
    public AudioClip playerDead;

    [Header("Death")]
    public GameObject gameOverPanel;

    private void Start()
    {
        health = playerHealth.InitialValue;
        healthBar.setMaxHealth(health); //Ngeset value ke script HealthBar
        tempX = -1;
        tempY = 0;
        rbd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>(); //Untuk animator
        sprite = GetComponent<SpriteRenderer>(); // Untuk Sprite
        audio = GetComponent<AudioSource>(); // Untuk sound
    }
    void Update()
    {
        healthBar.set(health);
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
            float knocks = 10f;
            float dmgTrap = 1f;
            Rigidbody2D hit = this.GetComponent<Rigidbody2D>();
            //Untuk Knockback
            if (hit != null)
            {
                Vector2 forceDirection = hit.transform.position - collision.transform.position;
                Vector2 force = forceDirection.normalized * knocks;
                hit.velocity = force;
                takeDamage(dmgTrap);
            }
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
            audio.PlayOneShot(playerWalk);
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
        if (canAttack && !UIManager.isPaused)
        {
            if (Inventory.myInventory.Contains(Sword))
            {
                durSword.SetActive(true);
                if(durabiltySword.RuntimeValue <= 0)
                {
                    Inventory.myInventory.Remove(Sword);
                    durSword.SetActive(false);
                }
                if (Input.GetKeyDown(KeyCode.M))
                {
                    Melee();
                }
            }
            if (Inventory.myInventory.Contains(Gun))
            {
                bullText.SetActive(true);
                if(bullets.RuntimeValue <= 0)
                {
                    Inventory.myInventory.Remove(Gun);
                    bullText.SetActive(false);
                }
                if (Input.GetMouseButton(0))
                {
                    Shooting();
                }
            }
        }
    }

    void Melee()
    {
        canAttack = false;
        anim.SetTrigger("Attack");
        audio.PlayOneShot(playerAttack);

        Collider2D[] hitEnemy = Physics2D.OverlapCircleAll(AttackPoint.position, AttackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemy)
        {
            durabiltySword.RuntimeValue -= 1;
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
        bullets.RuntimeValue -= 1;
        canAttack = false;
        anim.SetTrigger("Shooting");
        audio.PlayOneShot(playerShoot); //Play sound hanya 1x tiap dijalankan methodnya
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
        healthBar.setHealth(dmg);
        if(health <= 0)
        {
            UIManager.isDeath = true;
            gameOverPanel.SetActive(true);
            audio.PlayOneShot(playerDead);
            anim.SetTrigger("Death");
            StartCoroutine(DeathCo());
        }
    }

    IEnumerator DeathCo()
    {
        yield return new WaitForSeconds(1f);
        this.gameObject.SetActive(false);
    }

}
