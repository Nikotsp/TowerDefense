using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eletronic : Tower
{
    Transform shootTra;
    Monster lastTarget;
    LaserBullet curBullet;
    protected override void Awake()
    {
        shootTra = transform.Find("ShotPoint");
    }

    protected override void LookAt()
    {

    }
    protected override void Update()
    {
        //先判断有无target
        if (target == null)
        {
            target = FindTarget();
            if (target != null && curBullet != null)
                curBullet.gameObject.SetActive(true);
        }
        else
        {
            if (target.IsDead || Vector3.Distance(target.transform.position, transform.position) > GuardRange)
            {
                target = null;
                //curBullet.gameObject.SetActive(false);
            }
        }


        if (target != null)
        {
            if (target != lastTarget)
            {
                if (curBullet != null)
                {
                    curBullet.gameObject.SetActive(false);
                    //curBullet = null;
                }
                Attack();
                lastTarget = target;
            }
        }
        else
        {
            if (curBullet != null)
            {
                curBullet.gameObject.SetActive(false);
                //curBullet = null;
                lastTarget = null;
            }
        }
    }

    protected override void Attack()
    {
        base.Attack();
        GameObject go = Game.Instance.ObjectPool.Spawn("LaserBullet");
        LaserBullet laserBullet = go.GetComponent<LaserBullet>();
        laserBullet.transform.position = transform.position;
        curBullet = laserBullet;
        laserBullet.Load(this.UseBulletID, this.Level, this.map_Rect, target);
    }
}
