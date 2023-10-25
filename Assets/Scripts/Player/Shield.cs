using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    [SerializeField] float maxShield = 10f;
    [SerializeField] float burnSpeed = 2.5f;
    [SerializeField] float chargeSpeed = 1f;
    [SerializeField] float cooldown = 3f;

    [SerializeField] Slider chargeBar;
    [SerializeField] Animator barAnim;
   
    [Header("Audio")]
    [SerializeField] AudioClip openSound;
    [SerializeField] AudioClip hitSound;
    [SerializeField] AudioClip breakSound;
    [SerializeField] AudioClip brokenSound;

    float shieldCharge;

    public enum ShieldStatus
    {
        ACTIVE,
        CHARGING,
        BROKEN
    };

    ShieldStatus status = ShieldStatus.CHARGING;
    PlayerController controller;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        shieldCharge = maxShield;
        controller = GetComponentInParent<PlayerController>();
        anim = GetComponent<Animator>();

        chargeBar.maxValue = maxShield;
        chargeBar.value = shieldCharge;
    }

    // Update is called once per frame
    void Update()
    {
        switch (status)
        {
            case ShieldStatus.ACTIVE:
                shieldCharge -= burnSpeed * Time.deltaTime;
                if (shieldCharge <= 0.05f) 
                {
                    anim.SetBool("active", false);
                    anim.SetTrigger("break");
                    barAnim.SetBool("broken", true);
                    
                    controller.PlayAudio(breakSound);

                    status = ShieldStatus.BROKEN;
                    shieldCharge = 0;
                    StartCoroutine(Cooldown());
                }

                if (shieldCharge <= maxShield / 3) { GetComponent<SpriteRenderer>().color = new Color(179 / 255.0f, 143 / 255.0f, 255 / 255.0f); }
                else { GetComponent<SpriteRenderer>().color = Color.white; }
                break;
            case ShieldStatus.CHARGING:
                shieldCharge += chargeSpeed * Time.deltaTime;
                break;
            case ShieldStatus.BROKEN:
                
                break;
        }

        shieldCharge = Mathf.Clamp(shieldCharge, 0, maxShield);
        chargeBar.value = shieldCharge;

        if (GameManager.state == GameManager.GameState.WAITING) { chargeBar.gameObject.SetActive(false); }
        else { chargeBar.gameObject.SetActive(true); }
    }

    public void Activate()
    {
        if (status == ShieldStatus.BROKEN) 
        {
            anim.SetBool("active", false);
            controller.PlayAudio(brokenSound);
            return; 
        }

        if (status == ShieldStatus.CHARGING) { status = ShieldStatus.ACTIVE; }

        anim.SetBool("active", true);
        controller.PlayAudio(openSound);
    }

    public void Deactivate()
    {
        if (status == ShieldStatus.ACTIVE) { status = ShieldStatus.CHARGING; }
        anim.SetBool("active", false);
    }

    public void DamageShield(int value)
    {
        shieldCharge -= value;
        controller.PlayAudio(hitSound);
        GetComponent<Animator>().SetTrigger("hit");
    }

    public void AddCharge(float value)
    {
        if (status == ShieldStatus.BROKEN) 
        { 
            status = ShieldStatus.CHARGING;
            barAnim.SetBool("broken", false);
        }
        shieldCharge += value;
    }

    public float ChargePercentage()
    {
        return shieldCharge / maxShield;
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        status = ShieldStatus.CHARGING;
        barAnim.SetBool("broken", false);
    }
}
