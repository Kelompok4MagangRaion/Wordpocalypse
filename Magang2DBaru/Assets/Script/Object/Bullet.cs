using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;
    public float lifetime;
    private float lifetimeCounter;

    void Start()
    {
        lifetimeCounter = lifetime;
    }

    void Update()
    {
        lifetimeCounter -= Time.deltaTime; 
        if(lifetimeCounter <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void setup(Vector2 velocity, Vector3 distance)
    {
        rb.velocity = velocity.normalized * speed;
        transform.rotation = Quaternion.Euler(distance);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy"))
        {
            Destroy(this.gameObject);
        }
    }
}
