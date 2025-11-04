using UnityEngine;
using System.Collections;

public class Bottle : Tower
{
    Transform shootTf;
    protected override void Awake()
    {
        shootTf = transform.Find("ShotPoint");
    }
    protected override void Attack()
    {
        base.Attack();
        GameObject go = Game.Instance.ObjectPool.Spawn("BallBullet");
        BallBullet ballBullet = go.GetComponent<BallBullet>();
        ballBullet.transform.position = shootTf.transform.position;
        ballBullet.Load(this.UseBulletID, this.Level, this.map_Rect, target);
    }
}