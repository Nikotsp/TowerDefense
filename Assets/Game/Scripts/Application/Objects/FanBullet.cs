using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FanBullet : Bullet
{
    public float RotateSpeed = 180f;
    public Vector2 Direction;
    public void Load(int bulletID, int level, Rect mapRect, Vector3 direction)
    {
        base.Load(bulletID, level, mapRect);
        Direction = direction;
    }
    public override void Update()
    {
        base.Update();
        if (m_IsExplode)
            return;
        transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward, RotateSpeed * Time.deltaTime);
        GameObject[] monsterObjects = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject go in monsterObjects)
        {
            Monster monster = go.GetComponent<Monster>();
            if (monster.IsDead)
                continue;
            if (Vector3.Distance(go.transform.position, transform.position) < Consts.RangeClosedDistance)
            {
                monster.Damage((int)Attack);
                Explode();
                break;
            }
        }
        if (!Map_Rect.Contains(transform.position) && !m_IsExplode)
        {
            Explode();
        }
    }
}