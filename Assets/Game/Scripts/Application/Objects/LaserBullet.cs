using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class LaserBullet : Bullet
{
    Tower tower;
    Monster target;
    Vector3 Direction;
    Tweener rotateTweener;
    Tweener scaleTweener;
    float damageRate;
    float lastDamageTime;
    public void Load(int bulletID, int level, Rect mapRect, Monster target)
    {
        base.Load(bulletID, level, mapRect);
        this.target = target;
        rotateTweener = transform.DORotate(Vector3.zero, 0);
        scaleTweener = transform.DOScale(Vector3.one, 0);

        damageRate = 2f * level;
        lastDamageTime = Time.time;

    }
    public override void Update()
    {
        base.Update();
        if (m_IsExplode) return;
        if (target != null)
        {
            if (!target.IsDead)
            {
                //旋转
                Direction = (target.Position - transform.position).normalized;
                float angle = Mathf.Atan2(Direction.y, Direction.x) * Mathf.Rad2Deg - 90;
                Vector3 targetRotation = new Vector3(0, 0, angle);

                //缩放
                float distance = Vector3.Distance(target.Position, transform.position);
                float targetSacleY = distance / 2.26f;
                Vector3 targetScale = new Vector3(transform.localScale.x, targetSacleY, transform.localScale.z);

                rotateTweener.Kill();
                rotateTweener = transform.DORotate(targetRotation, 0.05f).SetEase(Ease.OutQuad);

                scaleTweener.Kill();
                scaleTweener = transform.DOScale(targetScale, 0.1f).SetEase(Ease.Linear);
            }
            if (Time.time >= lastDamageTime + 1 / damageRate)
            {
                target.Damage((int)Attack);
                lastDamageTime = Time.time;
            }
        }
    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();
        rotateTweener?.Kill();
        scaleTweener?.Kill();
        DOTween.Clear(false);
    }
}
