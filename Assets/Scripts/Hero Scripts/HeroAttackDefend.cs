using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttackDefend : MonoBehaviour
{
    private ItemDurability itemDurability;
    private Animator swordAnimator;
    public GameObject sword;
    public GameObject attackHitbox;
    private SpriteRenderer hbRender;
    public float hitboxOpacity;
    private bool attacking;

    // Start is called before the first frame update
    void Start()
    {
        hbRender = attackHitbox.GetComponent<SpriteRenderer>();
        itemDurability = GetComponent<ItemDurability>();
        swordAnimator = sword.GetComponent<Animator>();
        attacking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.X) && itemDurability.canUseShield() && !attacking)
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
        float swingTime = 0.29f;

        sword.SetActive(true);
        attackHitbox.SetActive(true);
        Color startColor = new Color(hbRender.color.r, hbRender.color.g, hbRender.color.b, hitboxOpacity);
        Color endColor = new Color(hbRender.color.r, hbRender.color.g, hbRender.color.b, 0);
        hbRender.color = startColor;
        swordAnimator.Play("SwordSwing");
        float timer = 0f;
        while (timer < swingTime)
        {
            timer += Time.deltaTime;
            hbRender.color = Vector4.Lerp(startColor, endColor, timer / swingTime);
            yield return null;
        }
        sword.SetActive(false);
        yield return new WaitForSeconds(0.04f);
        attackHitbox.SetActive(false);
        attacking = false;
    }

}
