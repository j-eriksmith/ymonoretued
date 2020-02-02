using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public float deadzone = 0.1f;
    public float speed = 5;
    public int maxHealth; // May want to use an even number to support half-hearts

    private int health;

    public float walkAnimBaseSpeed;
    public float walkAnimHighSpeed;
    public float walkAnimRampTime;

    private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;
    private float vertical;
    private float curRampTime;
    private bool healthImmune; // Immune when blocking or just after getting hit

    private bool moving, hasInputThisFrame;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        curRampTime = 0f;
        health = maxHealth;
        moving = false;
        hasInputThisFrame = false;
        healthImmune = false;
    }

    void Update()
    {
        rb.velocity = Vector2.zero;
        horizontal = Input.GetAxis("Hero Horizontal");
        vertical = Input.GetAxis("Hero Vertical");

        if (Mathf.Abs(horizontal) > deadzone)
        {
            hasInputThisFrame = true;
            moving = true;
            rb.velocity += new Vector2(horizontal * speed, 0);
        }

        if (Mathf.Abs(vertical) > deadzone)
        {
            hasInputThisFrame = true;
            moving = true;
            rb.velocity -= new Vector2(0, vertical * speed);
        }

        if (!hasInputThisFrame) moving = false;
        hasInputThisFrame = false;

        if (moving)
        {
            curRampTime += Time.deltaTime;
            if (curRampTime > walkAnimRampTime) curRampTime = walkAnimRampTime;
            animator.speed = Mathf.Lerp(walkAnimBaseSpeed, walkAnimHighSpeed, curRampTime / walkAnimRampTime);
        }
        else
        {
            curRampTime = 0f;
            animator.speed = walkAnimBaseSpeed;
        }

        if (rb.velocity.magnitude > 0)
        {
            Vector2 ball = new Vector2(-rb.velocity.y, rb.velocity.x);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(0, 0, 1f), ball), 0.1f);
        }
        else
        {
            rb.angularVelocity = 0f;
        }

        // Pullback();
    }

    protected virtual void Pullback()
    {
        Vector3 p = transform.position;
        p.x = Mathf.Min(Bounds.rect.xMax, Mathf.Max(Bounds.rect.xMin, p.x));
        p.y = Mathf.Min(Bounds.rect.yMax, Mathf.Max(Bounds.rect.yMin, p.y));
        transform.position = p;
    }

    public void DamagePlayer(int dmg)
    {
        health -= dmg;
        if (health <= 0) Die();
    }

    public int GetHealth()
    {
        return health;
    }

    private void Die()
    {

    }
}
