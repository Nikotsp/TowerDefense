using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using UnityEngine;

public abstract class Tower : ReusbleObject, IReusable
{
    int m_Level;
    protected Animator animator;
    protected Monster target;
    public Tile m_Tile;
    public Rect map_Rect;
    float m_LastAttackTime;
    public int ID { get; private set; }
    public int MaxLevel { get; private set; }
    public bool IsTopLevel { get { return Level >= MaxLevel; } }
    public int Level
    {
        get { return m_Level; }
        set
        {
            m_Level = Mathf.Clamp(value, 0, MaxLevel);
            transform.localScale = Vector3.one * (1 + m_Level * 0.25f);
        }
    }
    public int BasePrice { get; private set; }
    public int Price
    {
        get
        {
            return BasePrice * Level;
        }
    }
    public float GuardRange { get; private set; }
    public float ShotRate { get; private set; }
    public int UseBulletID { get; private set; }
    protected virtual void Awake()
    {

    }
    public void Load(int towerID, Tile tile, Rect MapRect)
    {
        TowerInfo towerInfo = Game.Instance.StaticData.GetTowerInfo(towerID);
        this.ID = towerInfo.ID;
        this.MaxLevel = towerInfo.MaxLevel;
        this.BasePrice = towerInfo.BasePrice;
        this.ShotRate = towerInfo.ShotRate;
        this.UseBulletID = towerInfo.UseBulletID;
        this.GuardRange = towerInfo.GuardRange;
        this.Level = 1;
        this.m_Tile = tile;
        this.map_Rect = MapRect;
    }

    protected Monster FindTarget()
    {
        Monster target = null;
        GameObject[] monseters = GameObject.FindGameObjectsWithTag("Monster");

        foreach (GameObject monster in monseters)
        {
            Monster ms = monster.GetComponent<Monster>();
            if (!ms.IsDead && Vector3.Distance(transform.position, monster.transform.position) <= GuardRange)
            {
                target = ms;
                break;
            }
        }

        return target;
    }

    protected virtual void Update()
    {
        if (target == null)
        {
            target = FindTarget();
        }
        else
        {
            if (Vector3.Distance(target.transform.position, transform.position) > GuardRange || target.IsDead)
            {
                target = null;
                m_LastAttackTime = 0;
                return;
            }
            float attakTime = m_LastAttackTime + 1 / ShotRate;
            if (Time.time >= attakTime)
            {
                Attack();
                m_LastAttackTime = Time.time;
            }

        }
        LookAt();
    }

    protected virtual void LookAt()
    {
        if (target == null)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = 0;
            transform.eulerAngles = eulerAngles;
        }
        else
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(dir.y, dir.x);
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = angle * Mathf.Rad2Deg - 90;
            transform.eulerAngles = eulerAngles;
        }
    }

    protected virtual void Attack() { }
    public override void OnSpawn()
    {
        animator = GetComponent<Animator>();
        animator.Play("Idle");

    }
    public override void OnUnspawn()
    {
        animator.ResetTrigger("IsAttack");
        animator.Play("Idle");
        target = null;
        m_Tile = null;
        GuardRange = 0;
        MaxLevel = 0;
        BasePrice = 0;
        ShotRate = 0;
        UseBulletID = 0;
        Level = 0;
        ID = 0;
        m_LastAttackTime = 0;
    }
}