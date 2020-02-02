using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnmity : Enmity
{
    private Animator animator;
    private bool attacking;

    new void Start()
    {
        base.Start();
        attacking = false;
        animator = GetComponent<Animator>();
    }
    protected override void Move()
    {
        if (attacking) return;
        Vector2 delta = (Vector2)(hero.transform.position - transform.position);
        if (!InRange())
            rb.velocity = delta;
        else
            rb.velocity = Vector2.zero;
    }

    protected override void Attack()
    {
        if (!InRange() || cooldown > 0.0f)
            return;
        Debug.Log("Attack!");
        attacking = true;
        StartCoroutine(spiderHop());
        cooldown = cooldownTime;
    }

    IEnumerator spiderHop()
    {
        animator.Play("spiderHop");
        yield return new WaitForSeconds(1.083f);
        Vector2 delta = (Vector2)(hero.transform.position - transform.position);
        rb.velocity = delta * 10;
        yield return new WaitForSeconds(0.41f);
        rb.velocity = Vector2.zero;
        attacking = false;
    }
}
