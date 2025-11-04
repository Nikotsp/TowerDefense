using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Luobo : Role
{
    public Animator animator;
    public override void Damage(int hit)
    {
        base.Damage(hit);

        if (IsDead)
            return;
        animator.SetTrigger("IsDamage");
    }
    public override void Die(Role role)
    {
        base.Die(role);
        animator.SetBool("IsDead", true);
    }
    public override void OnSpawn()
    {
        Debug.Log(1);
        base.OnSpawn();
        animator = GetComponent<Animator>();
        animator.Play("Idle");

        LuoboInfo info = Game.Instance.StaticData.GetLuoboInfo();
        MaxHp = info.Hp;
        Hp = info.Hp;
    }
    public override void OnUnspawn()
    {
        base.OnUnspawn();
        animator.SetBool("IsDead", false);
        animator.ResetTrigger("IsDamage");
    }
}