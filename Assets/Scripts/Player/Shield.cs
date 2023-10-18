using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] float maxShield = 10f;
    [SerializeField] float burnSpeed = 2.5f;
    [SerializeField] float chargeSpeed = 1f;
    [SerializeField] float cooldown = 3f;

    [SerializeField] AudioClip openSound;

    float shieldCharge;
    bool active = false;

    public enum ShieldStatus
    {
        ACTIVE,
        CHARGING,
        BROKEN
    };

    ShieldStatus status;

    PlayerController controller;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        shieldCharge = maxShield;
        controller = GetComponentInParent<PlayerController>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            shieldCharge -= burnSpeed * Time.deltaTime;
        }
        else
        {
            shieldCharge += chargeSpeed * Time.deltaTime;
        }

        shieldCharge = Mathf.Clamp(shieldCharge, 0, maxShield);
    }

    public void Activate()
    {
        active = true;
        if (status == ShieldStatus.CHARGING) { status = ShieldStatus.ACTIVE; }

        anim.SetBool("active", active);
        controller.PlayAudio(openSound);
    }

    public void Deactivate()
    {
        active = false;
        if (status == ShieldStatus.ACTIVE) { status = ShieldStatus.CHARGING; }
        anim.SetBool("active", active);
    }

    public void DamageShield(int value)
    {
        shieldCharge -= value;
    }
}
