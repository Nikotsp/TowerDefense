using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using System.Text;
using System.IO;
public static class Utils
{

    //加载配置表文件，解析内容 输出对应的Level对象
    public static void LoadLevel(string path, ref Level level)
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlElement root = doc.DocumentElement;
        level.Name = root.SelectSingleNode("Name").InnerText;
        level.CardImage = root.SelectSingleNode("CardImage").InnerText;
        level.Background = root.SelectSingleNode("Background").InnerText;
        level.Road = root.SelectSingleNode("Road").InnerText;
        level.InitScore = int.Parse(root.SelectSingleNode("InitScore").InnerText);

        //读取可放置区域
        XmlNodeList holderNodeList = root.SelectNodes("Holder/Point");
        foreach (XmlNode node in holderNodeList)
        {
            Point p = new(
                int.Parse(node.Attributes["X"].Value),
                int.Parse(node.Attributes["Y"].Value)
                );
            level.Holder.Add(p);
        }

        //读取寻路区域
        XmlNodeList pathNodeList = root.SelectNodes("Path/Point");
        foreach (XmlNode node in pathNodeList)
        {
            Point p = new(
                int.Parse(node.Attributes["X"].Value),
                int.Parse(node.Attributes["Y"].Value)
                );
            level.Path.Add(p);
        }
        //读取刷怪信息
        XmlNodeList roundNodeList = root.SelectNodes("Rounds/Round");
        foreach (XmlNode node in roundNodeList)
        {
            Round r = new(
                int.Parse(node.Attributes["Monster"].Value),
                int.Parse(node.Attributes["Count"].Value)
                );
            level.Rounds.Add(r);
        }
    }

}
