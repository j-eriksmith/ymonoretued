using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public float deadzone = 0.1f;
    public float speed = 100;
    public int maxHealth; // May want to use an even number to support half-hearts

    private int curHealth;

    public float walkAnimBaseSpeed;
    public float walkAnimHighSpeed;
    public float walkAnimRampTime;

    private Rigidbody2D rb;
    private Animator animator;
    private float horizontal;
    private float vertical;
    private float curRampTime;

    private bool moving, hasInputThisFrame;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        curRampTime = 0f;
        curHealth = maxHealth;
        moving = false;
        hasInputThisFrame = false;
    }

    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > deadzone)
        {
            hasInputThisFrame = true;
            moving = true;
            float stepSize = horizontal * speed * Time.deltaTime;
            rb.position += new Vector2(stepSize, 0);
        }

        if (Mathf.Abs(vertical) > deadzone)
        {
            hasInputThisFrame = true;
            moving = true;
            float stepSize = vertical * speed * Time.deltaTime;
            rb.position += new Vector2(0, stepSize);
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

        Pullback();
    }

    protected virtual void Pullback()
    {
        Vector3 p = transform.position;
        p.x = Mathf.Min(Bounds.rect.xMax, Mathf.Max(Bounds.rect.xMin, p.x));
        p.y = Mathf.Min(Bounds.rect.yMax, Mathf.Max(Bounds.rect.yMin, p.y));
        transform.position = p;
    }

    public void damagePlayer(int dmg)
    {
        curHealth -= dmg;
        // if (curHealth <= 0) ; // die
    }
}
