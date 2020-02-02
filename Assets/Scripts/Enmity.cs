using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public abstract class Enmity : MonoBehaviour
{
    protected Rigidbody2D rb;
    public GameObject hero;

    public float cooldownTime;
    public float range;
    public float moveSpeed;
    public int maxHealth;

    protected float cooldown;
    protected int health;

    private bool blockMovement; // Also used for enemy I-frames
    private VisualEffect splat;
    private CircleCollider2D circleCollide;

    // Start is called before the first frame update
    protected void Start()
    {
        health = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        hero = GameObject.FindWithTag("Hero");
        Debug.Log("Enemy spawned!");
        cooldown = cooldownTime;
        blockMovement = false;

        circleCollide.GetComponent<CircleCollider2D>();

        splat = GetComponentInChildren<VisualEffect>();
        splat.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        Orient();
        if (!blockMovement)
        {
            Move();
            Attack();
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

    protected virtual void Orient()
    {
        Vector3 delta = hero.transform.position - transform.position;
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(delta.y, delta.x));
    }

    protected virtual bool InRange()
    {
        Vector3 delta = hero.transform.position - transform.position;
        return delta.magnitude < range;
    }

    // should call InRange to determine how to move (speed up/slow down/hold)
    protected abstract void Move();

    // should call InRange to determine whether to attack
    // should use `cooldown`
    protected abstract void Attack();

    public void HitByPlayer()
    {
        if (blockMovement) return;
        blockMovement = true;
        --health;
        StartCoroutine(blockMovementForDuration(0.8f));
        Vector3 delta = hero.transform.position - transform.position;

        delta *= -25.5f / delta.magnitude * 2.5f / (2f + delta.magnitude);

        rb.velocity = delta;

        if (health <= 0) StartCoroutine(Die());
    }

    protected IEnumerator Die()
    {
        circleCollide.enabled = false;
        GetComponent<SpriteRenderer>().color = new Vector4(1f, 0f, 0f, 1f);
        splat.Play();
        Destroy(gameObject, 5f);
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().enabled = false;
    }

    IEnumerator blockMovementForDuration(float duration)
    {
        blockMovement = true;
        yield return new WaitForSeconds(duration);
        blockMovement = false;
    }
}
