using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEditor.Experimental.GraphView;
using Unity.VisualScripting;
using JetBrains.Annotations;
public class EletronicBullet : Bullet
{
    LineRenderer lineRenderer;
    Transform shootTra;
    Monster target;
    float damageRate;
    float lastDamageTime;
    public override void Awake()
    {
        base.Awake();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.useWorldSpace = true;
        lineRenderer.numCapVertices = 5;
        lineRenderer.numCornerVertices = 5;

        //2.26   distance / 2.26
    }
    public void Load(int bulletID, int level, Rect mapRect, Monster target, Transform shootTra)
    {
        base.Load(bulletID, level, mapRect);
        this.target = target;
        this.shootTra = shootTra;
        lineRenderer.material = Game.Instance.StaticData.GetLaserMateria();
        damageRate = 2f * level;
        lastDamageTime = Time.time;
    }
    public override void Update()
    {
        base.Update();
        if (m_IsExplode) return;
        //更新线段位置
        UpdateLaserLine();
        if (Time.time >= lastDamageTime + 1 / damageRate)
        {
            target.Damage((int)Attack);
            lastDamageTime = Time.time;
        }
    }

    private void UpdateLaserLine()
    {
        if (target == null) return;
        Vector3 starPos = shootTra.position;
        Vector3 endPos = target.transform.position;

        lineRenderer.SetPosition(0, starPos);
        lineRenderer.SetPosition(1, endPos);
        // 可选：添加长度动画（通过线宽微动画增强视觉效果）
        lineRenderer.widthMultiplier = Mathf.Lerp(0.2f, 0.2f + 0.05f, Mathf.PingPong(Time.time, 1f));

    }

    public override void OnUnspawn()
    {
        base.OnUnspawn();
        target = null;
        shootTra = null;
        lineRenderer.SetPosition(0, Vector3.zero);
        lineRenderer.SetPosition(1, Vector3.zero);
        lineRenderer.widthMultiplier = 0.2f; // 重置线宽
        lastDamageTime = 0;
    }
}
