﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDurability : MonoBehaviour
{
    public int maxSwordDurability, maxShieldDurability;

    private int curSwordDurability, curShieldDurability;

    // Start is called before the first frame update
    void Start()
    {
        curSwordDurability = maxShieldDurability;
        curShieldDurability = maxShieldDurability;
    }

    public bool canUseSword()
    {
        return curSwordDurability > 0;
    }

    public bool canUseShield()
    {
        return curShieldDurability > 0;
    }

    public int getSwordDurability()
    {
        return curSwordDurability;
    }

    public int getShieldDurability()
    {
        return curShieldDurability;
    }

    public void damageSword(int dmg)
    {
        curSwordDurability -= dmg;
    }

    public void damageShield(int dmg)
    {
        curShieldDurability -= dmg;
    }

    public void repairSword(int amt)
    {
        curSwordDurability += amt;
    }

    public void repairShield(int amt)
    {
        curShieldDurability += amt;
    }
}