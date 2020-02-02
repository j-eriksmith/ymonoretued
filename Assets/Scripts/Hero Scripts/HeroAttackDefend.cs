using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackDefend : MonoBehaviour
{
    private ItemDurability itemDurability;
    private Animator swordAnimator;
    public GameObject sword;
    private bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        itemDurability = GetComponent<ItemDurability>();
        swordAnimator = sword.GetComponent<Animator>();
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W) && itemDurability.canUseShield() && !attacking)
        {

        }

        else if (Input.GetKeyDown(KeyCode.Z) && itemDurability.canUseSword() && !attacking)
        {
            attacking = true;
            StartCoroutine(SwordAttack());
        }
    }

    IEnumerator SwordAttack()
    {
        sword.SetActive(true);
        swordAnimator.Play("SwordSwing");
        yield return new WaitForSeconds(0.26f);
        sword.SetActive(false);
        yield return new WaitForSeconds(0.04f);
        attacking = false;
    }

}
