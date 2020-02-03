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

    public GameObject dropOffZone;
    private DropoffZoneController dropoffZoneController;

    private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;
    private float vertical;
    private float curRampTime;
    private bool healthImmune; // Immune when blocking or just after getting hit
    private CircleCollider2D hitbox;

    private bool moving, hasInputThisFrame;
    public bool hasSword, hasShield;

    public float invulDuration = 1;
    float invulCooldown = 0;
    bool invulnerable = false;

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
        hasSword = true;
        hasShield = true;
        hitbox = GetComponent<Transform>().Find("Hitbox").GetComponent<CircleCollider2D>();
        dropoffZoneController = dropOffZone.GetComponent<DropoffZoneController>();
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

        // Triggers against enemies, hero takes damage
        if(!invulnerable && hitbox.IsTouchingLayers(LayerMask.GetMask("Enemy"))){
            DamagePlayer(1);
            GetComponent<SpriteRenderer>().color = Color.red;
            invulCooldown = invulDuration;
            invulnerable = true;
        }

        if(invulnerable){
            if(invulCooldown <= 0){
                invulnerable = false;
                GetComponent<SpriteRenderer>().color = Color.white;
            }

            invulCooldown -= Time.deltaTime;
        }
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
        print("You took damage, your life is now " + health);
        if (health <= 0) Die();
    }

    public int GetHealth()
    {
        return health;
    }

    private void Die()
    {
        print("Omae wa mou shindeiru");
    }

    private string GetAnimName(string str)
    {
        return "HeroIdle" + str;
    }

    public void dropOffSword()
    {
        hasSword = false;
        if (hasShield) animator.Play(GetAnimName("Shield"));
        else animator.Play(GetAnimName(""));
        dropoffZoneController.Dropoff(gameObject.GetComponent<ItemDurability>().getSwordDurability());
    }

    public void pickUpSword()
    {
        hasSword = true;
        if (hasShield) animator.Play(GetAnimName("Swsh"));
        else animator.Play(GetAnimName("Sword"));
        gameObject.GetComponent<ItemDurability>().setSwordDurability(dropoffZoneController.);
        dropoffZoneController.Pickup();
    }

    public void dropOffShield()
    {
        hasShield = false;
        if (hasSword) animator.Play(GetAnimName("Sword"));
        else animator.Play(GetAnimName(""));
    }

    public void pickUpShield()
    {
        hasShield = true;
        if (hasSword) animator.Play(GetAnimName("Swsh"));
        else animator.Play(GetAnimName("Shield"));
    }
}
