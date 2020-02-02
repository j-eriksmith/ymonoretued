﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enmity : MonoBehaviour
{
    protected Rigidbody2D rb;
    public GameObject hero;

    public float cooldownTime;
    public float range;
    public int maxHealth;

    protected float cooldown;
    protected int health;

    // Start is called before the first frame update
    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hero = GameObject.FindWithTag("Hero");
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
}
