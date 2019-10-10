﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class ItemMsg : ISerializable
{
    public int ID; // 道具的ID
    public string Name; // 道具的名字
    public List<int> Attribs; // 属性ID

    public void Serialize(CSerialize ar)
    {
        ar.ReadWriteValue(ref ID);
        ar.ReadWriteValue(ref Name);
        ar.SerializeBaseArray(ref Attribs);
    }
};

class ItemPackMsg
{
    public List<ItemMsg> Items;
    public void Serialize(CSerialize ar)
    {
        ar.SerializeStructArray(ref Items);
    }
}

class LoginMsg
{
    public string szUserName;// 账号
    public string szPassword;// 密码
    public int nPlatType; // 平台类型
    public string szPlatName; // 平台名字
    public void Serialize(CSerialize ar)
    {
        ar.ReadWriteValue(ref szUserName);
        ar.ReadWriteValue(ref szPassword);
        ar.ReadWriteValue(ref nPlatType);
        ar.ReadWriteValue(ref szPlatName);
    }
};

public class TestMsg : FCScriptLoader
{
    // 记录脚本的LOG
    protected override bool IsRecrodLog()
    {
        return true;
    }

    void TestFunc0()
    {
        LoginMsg msg = new LoginMsg();
        msg.szUserName = "我是小丸子";
        msg.szPassword = "爱你一万年";
        msg.nPlatType = 1;
        msg.szPlatName = "时空机";
        CSerialize ar = new CSerialize(SerializeType.write);
        msg.Serialize(ar);

        FCLibHelper.fc_serialize_msg_call(0, "TestSerialize.ReceiveLoginMsg", ar.GetBuffer(), 0, ar.GetBufferSize(), true);
    }
    void TestFunc1()
    {
        ItemPackMsg msg = new ItemPackMsg();
        msg.Items = new List<ItemMsg>();

        ItemMsg item = new ItemMsg();
        item.ID = 1;
        item.Name = "无忧草";
        msg.Items.Add(item);

        item = new ItemMsg();
        item.ID = 2;
        item.Name = "大力丸";
        msg.Items.Add(item);

        item = new ItemMsg();
        item.ID = 3;
        item.Name = "回生丸";
        msg.Items.Add(item);

        CSerialize ar = new CSerialize(SerializeType.write);
        msg.Serialize(ar);

        FCLibHelper.fc_serialize_msg_call(0, "TestSerialize.ReceiveItemMsg", ar.GetBuffer(), 0, ar.GetBufferSize(), true);
    }

    void OnGUI()
    {
        int nLeft = 200;
        int nTop = 200;
        nLeft = 200;
        nTop += 80;
        if (GUI.Button(new Rect(nLeft, nTop, 120.0f, 30.0f), "清空LOG"))
        {
            m_ScriptLog.Clear();
        }
        nLeft = 200;
        nTop += 80;
        if (GUI.Button(new Rect(nLeft, nTop, 120.0f, 30.0f), "测试Test0"))
        {
            TestFunc0();
        }
        nLeft += 160;
        if (GUI.Button(new Rect(nLeft, nTop, 120.0f, 30.0f), "测试Test1"))
        {
            TestFunc1();
        }
        float fy = 10.0f;
        float fWidth = Screen.width - fy - 10;
        List<string> aLog = ScriptLog;
        for (int i = 0; i < aLog.Count; ++i)
        {
            GUI.Label(new Rect(10.0f, fy, fWidth, 20.0f), aLog[i]);
            fy += 25;
        }
    }
}
