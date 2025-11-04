using UnityEngine;
using System.Collections;

public class Fan : Tower
{
    int Count = 6;
    protected override void Attack()
    {
        base.Attack();
        for (int i = 0; i < Count; i++)
        {
            float angle = (Mathf.PI * 2f / Count) * i;
            Vector3 dir = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0);

            GameObject go = Game.Instance.ObjectPool.Spawn("FanBullet");
            FanBullet bullet = go.GetComponent<FanBullet>();
            bullet.transform.position = transform.position;
            bullet.Load(this.UseBulletID, this.Level, this.map_Rect, dir);
        }
    }
}