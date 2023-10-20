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

    [SerializeField] AudioClip openSound;

    [SerializeField] Slider chargeBar;

    float shieldCharge;
    float cooldownTimer;

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
                    status = ShieldStatus.BROKEN;
                    shieldCharge = 0;
                    StartCoroutine(Cooldown());
                }
                break;
            case ShieldStatus.CHARGING:
                shieldCharge += chargeSpeed * Time.deltaTime;
                break;
            case ShieldStatus.BROKEN:
                break;
        }

        shieldCharge = Mathf.Clamp(shieldCharge, 0, maxShield);
        chargeBar.value = shieldCharge;
    }

    public void Activate()
    {
        if (status == ShieldStatus.BROKEN) 
        {
            anim.SetBool("active", false);
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
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
        status = ShieldStatus.CHARGING;
    }
}
