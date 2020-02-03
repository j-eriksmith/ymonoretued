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
    private bool defending;
    public GameObject shield;
    private bool shieldInvul;
    private float shieldInvulCooldown = 0.0f;
    public float shieldInvulDuration = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        hbRender = attackHitbox.GetComponent<SpriteRenderer>();
        itemDurability = GetComponent<ItemDurability>();
        swordAnimator = sword.GetComponent<Animator>();
        attacking = false;
        print(shield);
    }

    // Update is called once per frame
    void Update()
    {
        if (!defending && (Input.GetKeyDown(KeyCode.X) || Input.GetButtonDown("Hero B")) && itemDurability.canUseShield() && !attacking)
        {
            defending = true;
            // Durability stuff here
            StartCoroutine(BraceShield());
            //print("Blocking");
        }
        else if(Input.GetKeyUp(KeyCode.X) || Input.GetButtonUp("Hero B")){
            defending = false;
            StartCoroutine(LowerShield());
        }

        else if ((Input.GetKeyDown(KeyCode.Z) || Input.GetButtonDown("Hero A")) && itemDurability.canUseSword() && !attacking)
        {
            attacking = true;
            itemDurability.damageSword(itemDurability.damagePerSwing);
            StartCoroutine(SwordAttack());
        }
    }


    IEnumerator BraceShield(){
        shield.SetActive(true);
        
        if(!shieldInvul && shield.GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.GetMask("Enemy"))){
            shieldInvul = true;
            itemDurability.damageShield(1);
            shieldInvulCooldown = shieldInvulDuration;
        }

        if(shieldInvul){
            if(shieldInvulCooldown <= 0){
                shieldInvul = false;
            }

            shieldInvulCooldown -= Time.deltaTime;
        }
        yield return null;
    }

    IEnumerator LowerShield(){
        shield.SetActive(false);
        yield return null;
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
