using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enmity : MonoBehaviour
{
    public float cooldownTime;
    public float range;

    protected float cooldown;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Enemy spawned!");
        cooldown = cooldownTime;
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;
        Orient();
        Move();
        Attack();
    }

    protected virtual void Orient()
    {

    }

    protected virtual bool InRange()
    {
        // distance between me and player is less than `range`
        return false;
    }

    // should call InRange to determine how to move (speed up/slow down/hold)
    protected abstract void Move();

    // should call InRange to determine whether to attack
    // should use `cooldown`
    protected abstract void Attack();
}
