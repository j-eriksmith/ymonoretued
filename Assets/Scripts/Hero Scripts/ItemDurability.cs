using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDurability : MonoBehaviour
{
    public int damagePerSwing, damagePerHit, damagePerBlock;

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
        Debug.Log(curSwordDurability + " sword");
    }

    public void damageShield(int dmg)
    {
        curShieldDurability -= dmg;
        Debug.Log(curSwordDurability + " shield");
    }

    public void setSwordDurability(int amt)
    {
        curSwordDurability = amt;
    }

    public void setShieldDurability(int amt)
    {
        curShieldDurability = amt;
    }
}
