using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
public class ChangeRoundArgs : EventArgs
{
    public int CurRound;
    public ChangeRoundArgs(int curRound)
    {
        this.CurRound = curRound;
    }
}
public class RoundModel : Model
{

    public const float ROUND_INTERVAL = 3f;                                             //回合间隔时间
    public const float SPAWN_INTERVAL = 1f;                                            //出怪时间间隔
    List<Round> m_Rounds;                                                               //该关卡所有出怪信息
    int m_RoundIndex = -1;                                                             //当前回合索引
    bool m_AllRoundsCompelet = false;                                                  //是否所有怪物出完了
    Coroutine coroutine;                                                               //携程缓存

    #region 属性
    public override string Name => Consts.M_RoundModel;
    public int RoundIndex => m_RoundIndex;
    public int RoundTotal => m_Rounds.Count;
    public bool AllRoundsCompelet => m_AllRoundsCompelet;
    #endregion
    public event EventHandler<ChangeRoundArgs> OnChangeRound;

    #region 方法 
    //加载关卡 只用获得关键数据rounds
    public void LoadLevel(Level level)
    {
        m_Rounds = level.Rounds;
    }
    //开始回合 携程开始
    public void StartRound()
    {
        coroutine = Game.Instance.StartCoroutine(RunRound());
    }
    //回合暂停 协程暂停 缓存起来
    public void StopRound()
    {
        Game.Instance.StopCoroutine(coroutine);
        m_RoundIndex = -1;
        m_AllRoundsCompelet = false;
        m_Rounds = null;
    }
    //运行回合
    IEnumerator RunRound()
    {
        m_RoundIndex = -1;//当前回合默认值
        m_AllRoundsCompelet = false;                                                        //是否已经打完
        for (int i = 0; i < m_Rounds.Count; i++)                                            //第一层for 遍历所有回合数
        {
            m_RoundIndex = i;                                                             //设置当前回合
            ChangeRoundArgs cr = new ChangeRoundArgs(i + 1);
            OnChangeRound.Invoke(this, cr);
            StartRoundArgs startRoundArgs = new StartRoundArgs();                          //回合开始事件 事件参数：当前回合 总回合
            startRoundArgs.RoundIndex = m_RoundIndex;
            startRoundArgs.RoundTotal = RoundTotal;
            SendEvent(Consts.E_StartRound, startRoundArgs);
            Round round = m_Rounds[i];
            for (int j = 0; j < round.Count; j++)                                           //第二层for 执行单个round的内部
            {
                yield return new WaitForSeconds(SPAWN_INTERVAL);                            //出怪间隙
                SpawnMonsterArgs spawnMonsterArgs = new SpawnMonsterArgs();                 //出怪事件
                spawnMonsterArgs.MonsterID = round.Monster;
                SendEvent(Consts.E_SpawnMonster, spawnMonsterArgs);
                //最后一波出怪完成
                if ((i == m_Rounds.Count - 1) && (j == round.Count - 1))
                {
                    m_AllRoundsCompelet = true;
                }
            }

            if (!m_AllRoundsCompelet)
            {
                //回合间隙
                yield return new WaitForSeconds(ROUND_INTERVAL);
            }
        }
    }
    #endregion

}
