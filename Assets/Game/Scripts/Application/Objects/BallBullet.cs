using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class BallBullet : Bullet
{
    Monster target;
    Vector3 Direction;
    public void Load(int bulletID, int level, Rect mapRect, Monster monster)
    {
        base.Load(bulletID, level, mapRect);
        this.target = monster;
    }
    public override void Update()
    {
        base.Update();
        if (m_IsExplode)
            return;
        if (target != null)
        {
            if (!target.IsDead)
            {
                Direction = (target.transform.position - transform.position).normalized;
            }
            LookAt();
            transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
            if (Vector3.Distance(target.transform.position, transform.position) <= Consts.DotClosedDistance)
            {
                target.Damage((int)Attack);
                Explode();
            }
        }
        else
        {
            transform.Translate(Direction * Speed * Time.deltaTime, Space.World);
            if (!Map_Rect.Contains(transform.position) && !m_IsExplode)
            {
                Explode();
            }
        }
    }
    void LookAt()
    {
        float angle = Mathf.Atan2(Direction.y, Direction.x);
        Vector3 eularAngle = transform.eulerAngles;
        eularAngle.z = angle * Mathf.Rad2Deg - 90;
        transform.eulerAngles = eularAngle;
    }

}