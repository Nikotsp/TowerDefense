using UnityEngine;
using System.Collections;

public abstract class Bullet : ReusbleObject, IReusable
{
    public int ID { get; private set; }
    public int Level { get; private set; }
    public float BaseSpeed { get; private set; }
    public float BaseAttack { get; private set; }
    public float Speed { get { return BaseSpeed * Level; } }
    public float Attack { get { return BaseAttack * Level; } }
    //地图范围 回收子弹
    public Rect Map_Rect;
    //延迟回收时间
    public float DelayTime = 1f;
    //是否爆炸
    public bool m_IsExplode;
    //动画组件
    Animator animator;
    public virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public virtual void Update()
    {

    }
    public void Load(int bulletID, int level, Rect mapRect)
    {
        this.ID = bulletID;
        this.Level = level;

        BulletInfo bulletInfo = Game.Instance.StaticData.GetBulletInfo(ID);
        BaseSpeed = bulletInfo.BaseSpeed;
        BaseAttack = bulletInfo.BaseAttack;
        Map_Rect = mapRect;
    }
    protected void Explode()
    {
        m_IsExplode = true;
        animator.SetTrigger("IsExplode");
        StartCoroutine(DelayCoroutine(DelayTime));
    }
    IEnumerator DelayCoroutine(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        Game.Instance.ObjectPool.Unspawn(this.gameObject);
    }
    public override void OnSpawn()
    {

    }
    public override void OnUnspawn()
    {
        m_IsExplode = false;
        animator.Play("Play");
        animator.ResetTrigger("IsExplode");
    }
}
