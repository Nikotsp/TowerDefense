using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System;

[CustomEditor(typeof(Map))]
public class MapEditor : Editor
{
    [HideInInspector]
    public Map Map = null;

    //关卡列表
    List<FileInfo> m_files = new List<FileInfo>();
    private string[] modeToolBar = new string[] { "新建场景", "编辑关卡" };
    //当前编辑的关卡索引号
    int m_selectIndex = -1;
    int curMode = 0;
    int lastMode = -1;
    //公用字段
    string newLevelFileName = "";
    string levelName;
    string initScore;
    SerializedProperty CardImage;
    SerializedProperty Background;
    SerializedProperty RoadImage;

    int roundCount = 0;
    bool needRefresh = false;
    List<int> MonsterIdList = new List<int>();
    List<int> MonsterCountList = new List<int>();

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (Application.isPlaying)
        {
            //关联的Mono脚本组件
            Map = target as Map;
            EditorGUILayout.BeginHorizontal();
            int newMode = GUILayout.Toolbar(curMode, modeToolBar);
            if (newMode != curMode)
            {
                needRefresh = true;
                lastMode = curMode;
                curMode = newMode;
            }
            EditorGUILayout.EndHorizontal();
            CardImage = serializedObject.FindProperty("CardImage");
            Background = serializedObject.FindProperty("Background");
            RoadImage = serializedObject.FindProperty("RoadImage1");
            switch (curMode)
            {
                case 1:
                    OnEditMode();
                    break;
                case 0:
                    OnCreateMode();
                    break;
            }
        }

        if (GUI.changed)
            EditorUtility.SetDirty(target);
    }

    void OnCreateMode()
    {
        OnCommonFields(null, needRefresh);
        needRefresh = false;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("新建关卡"))
        {
            SaveLevel(true);
        }
        EditorGUILayout.EndHorizontal();
    }
    void OnEditMode()
    {
        EditorGUILayout.BeginHorizontal();
        int currentIndex = EditorGUILayout.Popup(m_selectIndex, GetNames(m_files));
        if (currentIndex != m_selectIndex)
        {
            m_selectIndex = currentIndex;

            //加载关卡
            LoadLevel();
            needRefresh = true;
        }
        if (GUILayout.Button("读取列表"))
        {
            //读取关卡列表
            LoadLevelFiles();
            needRefresh = true;
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("清除塔点"))
        {
            Map.ClearHolder();
        }
        if (GUILayout.Button("清除路径"))
        {
            Map.ClearRoad();
        }
        EditorGUILayout.EndHorizontal();


        OnCommonFields(Map.Level, needRefresh);
        if (needRefresh) needRefresh = false;
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("保存关卡"))
        {
            SaveLevel(false);
        }
        EditorGUILayout.EndHorizontal();
    }
    void OnCommonFields(Level level, bool needRefresh)
    {
        if (needRefresh)
        {
            if (level != null)
            {
                // 编辑模式：从level中读取数据
                newLevelFileName = m_selectIndex >= 0 && m_selectIndex < m_files.Count ? m_files[m_selectIndex].Name : "";
                levelName = level.Name;
                initScore = level.InitScore.ToString();
                roundCount = level.Rounds.Count;

                // CardImage.objectReferenceValue = AssetDatabase.LoadAssetAtPath<Texture2D>("/Game/Resources/Res/Cards" + level.CardImage);
                // serializedObject.ApplyModifiedProperties();
                // EditorUtility.SetDirty(CardImage.objectReferenceValue);


                // 加载怪物ID和数量列表
                MonsterIdList.Clear();
                MonsterCountList.Clear();
                foreach (var round in level.Rounds)
                {
                    MonsterIdList.Add(round.Monster);
                    MonsterCountList.Add(round.Count);
                }
            }
            else
            {
                // 新建模式：显示默认值
                newLevelFileName = "level" + (m_files.Count > 0 ? m_files.Count : 0) + ".xml";
                levelName = "";
                initScore = "";
                roundCount = 0;
                MonsterIdList.Clear();
                MonsterCountList.Clear();
            }
        }
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("关卡文件名:");
        newLevelFileName = GUILayout.TextField(newLevelFileName);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("关卡名字:");
        levelName = GUILayout.TextField(levelName);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("初始分数");
        initScore = GUILayout.TextField(initScore);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(CardImage);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.PropertyField(Background);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.PropertyField(RoadImage);
        serializedObject.ApplyModifiedProperties();


        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("回合信息：");
        GUILayout.Label("怪物总波数：");
        int newRoundCount = EditorGUILayout.IntField(roundCount);
        EditorGUILayout.EndHorizontal();

        if (newRoundCount != roundCount)
        {
            if (newRoundCount > roundCount)
            {
                for (int i = roundCount; i < newRoundCount; i++)
                {
                    MonsterIdList.Add(0);
                    MonsterCountList.Add(0);
                }
            }
            else
            {
                MonsterIdList.RemoveRange(newRoundCount, MonsterIdList.Count - newRoundCount);
                MonsterCountList.RemoveRange(newRoundCount, MonsterCountList.Count - newRoundCount);
            }
            roundCount = newRoundCount;
        }

        if (roundCount > 0)
        {
            for (int i = 0; i < roundCount; i++)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("怪物ID:");
                MonsterIdList[i] = EditorGUILayout.IntField(MonsterIdList[i]);

                GUILayout.Label("怪物数量:");
                MonsterCountList[i] = EditorGUILayout.IntField(MonsterCountList[i]);
                EditorGUILayout.EndHorizontal();
            }
        }

    }

    void LoadLevelFiles()
    {
        //清除状态
        Clear();

        //加载列表
        m_files = Tools.GetLevelFiles();

        //默认加载第一个关卡
        if (m_files.Count > 0)
        {
            m_selectIndex = 0;
            LoadLevel();
            needRefresh = true;
        }
    }

    void LoadLevel()
    {
        FileInfo file = m_files[m_selectIndex];

        Level level = new Level();
        Tools.FillLevel(file.FullName, ref level);

        Map.LoadLevel(level);
    }

    void SaveLevel(bool isNew)
    {
        //获取当前加载的关卡
        Level level = isNew ? new Level() : Map.Level;
        level.Name = levelName;
        //临时索引点
        List<Point> list = null;

        //收集放塔点
        list = new List<Point>();
        for (int i = 0; i < Map.Grid.Count; i++)
        {
            Tile t = Map.Grid[i];
            if (t.CanHold)
            {
                Point p = new Point(t.X, t.Y);
                list.Add(p);
            }
        }
        level.Holder = list;

        //收集寻路点
        list = new List<Point>();
        for (int i = 0; i < Map.Road.Count; i++)
        {
            Tile t = Map.Road[i];
            Point p = new Point(t.X, t.Y);
            list.Add(p);
        }
        level.Path = list;

        if (!int.TryParse(initScore, out level.InitScore))
        {
            EditorUtility.DisplayDialog("新建错误", "输入不合法", "确定");
            return;
        }
        if (CardImage.objectReferenceValue != null)
            level.CardImage = (CardImage.objectReferenceValue as Texture2D).name + ".png";
        if (Background.objectReferenceValue != null)
            level.Background = (Background.objectReferenceValue as Texture2D).name + ".png";
        if (RoadImage.objectReferenceValue != null)
            level.Road = (RoadImage.objectReferenceValue as Texture2D).name + ".png";
        level.Rounds.Clear();
        if (roundCount > 0)
        {
            for (int i = 0; i < roundCount; i++)
            {
                Round r = new Round(MonsterIdList[i], MonsterCountList[i]);
                level.Rounds.Add(r);
            }
        }
        string path;
        if (isNew)
        {
            // 新建模式：使用newLevelFileName作为文件名
            string fileName = newLevelFileName;
            if (string.IsNullOrEmpty(fileName))
            {
                EditorUtility.DisplayDialog("错误", "文件名不能为空", "确定");
                return;
            }
            path = Consts.LevelDir + fileName;
            if (File.Exists(path))
            {
                EditorUtility.DisplayDialog("保存失败", "文件已存在", "确定");
                return;
            }
        }
        else
        {
            // 编辑模式：直接使用选中文件的完整路径
            if (m_selectIndex < 0 || m_selectIndex >= m_files.Count)
            {
                EditorUtility.DisplayDialog("错误", "未选中任何关卡文件", "确定");
                return;
            }
            path = m_files[m_selectIndex].FullName; // 直接用原文件的完整路径
        }

        Tools.SaveLevel(path, level);
        if (isNew) LoadLevelFiles();
        EditorUtility.DisplayDialog("保存关卡数据", "保存成功", "确定");
        AssetDatabase.Refresh();
    }

    void Clear()
    {
        m_files.Clear();
        m_selectIndex = -1;
    }


    string[] GetNames(List<FileInfo> files)
    {
        List<string> names = new List<string>();
        foreach (FileInfo file in files)
        {
            names.Add(file.Name);
        }
        return names.ToArray();
    }
}