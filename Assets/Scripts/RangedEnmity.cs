using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnmity : Enmity
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
        delta *= moveSpeed / delta.magnitude;
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
        cooldown = cooldownTime;
    }
}
