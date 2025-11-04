using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : Role
{
    public const float CLOSED_DISTANCE = 0.1f;
    public event Action<Monster> Reached;
    public MonsterType MonsterType = MonsterType.Monster0;
    float m_MoveSpeed;//移动速度
    Vector3[] m_Path;//路径拐点
    int m_PointIndex = -1;//当前寻路点
    bool isReached;//是否到达终点
    int m_Price;
    public float MoveSpeed
    {
        get { return m_MoveSpeed; }
        set { m_MoveSpeed = value; }
    }
    public bool IsReached
    {
        get { return IsReached; }
    }
    public int Price
    {
        get { return m_Price; }
        set { m_Price = value; }
    }
    public Vector3 Position { get { return this.transform.position; } }
    public void Load(Vector3[] path)      //行进路线
    {
        m_Path = path;
        MoveNext();
    }

    bool HasNext()
    {
        return (m_PointIndex + 1) < (m_Path.Length - 1);
    }
    void MoveNext()
    {
        if (!HasNext())
            return;
        if (m_PointIndex == -1)
        {
            m_PointIndex = 0;
            MoveTo(m_Path[m_PointIndex]);
        }
        else
        {
            m_PointIndex++;
        }
    }
    void MoveTo(Vector3 position)
    {
        this.transform.position = position;
    }
    void Update()
    {
        if (isReached)
            return;
        Vector3 pos = this.transform.position;
        Vector3 des = m_Path[m_PointIndex + 1];
        float distance = Vector3.Distance(pos, des);
        if (distance <= CLOSED_DISTANCE)
        {
            MoveTo(des);
            if (HasNext())
                MoveNext();
            else
            {
                isReached = true;
                if (Reached != null)
                    Reached.Invoke(this);
            }
        }
        else
        {
            Vector3 dir = (des - pos).normalized;
            transform.Translate(dir * m_MoveSpeed * Time.deltaTime);
        }
    }

    //事件回调
    public override void OnSpawn()
    {
        base.OnSpawn();
        MonsterInfo info = Game.Instance.StaticData.GetMonsterInfo((int)MonsterType);
        this.MaxHp = info.Hp;
        this.Hp = info.Hp;
        this.MoveSpeed = info.MoveSpeed;
        this.Price = info.Price;
    }
    public override void OnUnspawn()
    {
        base.OnUnspawn();
        this.m_Path = null;
        this.m_PointIndex = -1;
        this.isReached = false;
        this.m_MoveSpeed = 0;
        this.Reached = null;
        this.m_Price = 0;

    }
}