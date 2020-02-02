using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnmity : Enmity
{
    protected override void Move()
    {
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
        Debug.Log("Ranged attack!");
        cooldown = cooldownTime;
    }
}
