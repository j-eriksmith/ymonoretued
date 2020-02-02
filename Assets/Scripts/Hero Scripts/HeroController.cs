using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    public float deadzone = 0.1f;
    public float speed = 100;
    public int maxHealth; // May want to use an even number to support half-hearts

    private int curHealth;

    private Rigidbody2D rb;
    private float horizontal;
    private float vertical;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        curHealth = maxHealth;
    }

    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > deadzone)
        {
            float stepSize = horizontal * speed * Time.fixedDeltaTime;
            rb.position += new Vector2(stepSize, 0);
        }

        if (Mathf.Abs(vertical) > deadzone)
        {
            float stepSize = vertical * speed * Time.fixedDeltaTime;
            rb.position += new Vector2(0, stepSize);
        }
    }

    public void damagePlayer(int dmg)
    {
        curHealth -= dmg;
        if (curHealth <= 0) ; // die
    }
}
