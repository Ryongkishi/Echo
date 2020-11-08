using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    //人物模型
    public GameObject humanPrefab;
    //人物列表
    public BaseHuman myhuman;
    public Dictionary<string, BaseHuman> otherHumans;

    // Start is called before the first frame update
    void Start()
    {
        NetManaager.AddListener("Enter", OnEnter);
        NetManaager.AddListener("OnMove", OnMove);
        NetManaager.AddListener("OnLeave", OnLeave);
        NetManaager.Connect("127.0.0.1", 8080);
        //添加一个角色
        GameObject obj = (GameObject)Instantiate(humanPrefab);
        float x = Random.Range(-5, 5);
        float z = Random.Range(-5, 5);
        obj.transform.position = new Vector3(x, 1, z);
        myhuman = obj.AddComponent<CtrlHuman>();
        myhuman.desc = NetManaager.GetDesc();
        //发送协议
        Vector3 pos = myhuman.transform.position;
        Vector3 eul = myhuman.transform.eulerAngles;
        string sendStr = "Enter|";
        sendStr += NetManaager.GetDesc() + ",";
        sendStr += pos.x + ",";
        sendStr += pos.y + ",";
        sendStr += pos.z + ",";
        sendStr += eul.y;
        NetManaager.Send(sendStr);
    }

    // Update is called once per frame
    void Update()
    {
        NetManaager.Update();
    }
    void OnEnter(string msg)
    {
        Debug.Log("OnEnter" + msg.ToString());
        //解析参数
        string[] split = msg.Split(',');
        string desc = split[0];
        float x = float.Parse(split[1]);
        float y = float.Parse(split[2]);
        float z = float.Parse(split[3]);
        float euly = float.Parse(split[4]);
        //是自己
        if (desc == NetManaager.GetDesc())
        {
            return;
        }
        //添加一个角色
        GameObject obj = (GameObject)Instantiate(humanPrefab);
        obj.transform.position = new Vector3(x, y, z);
        obj.transform.eulerAngles = new Vector3(0, euly, 0);
        BaseHuman hu = obj.AddComponent<SyncHuman>();
        hu.desc = desc;
        otherHumans.Add(desc, hu);
    }
    void OnMove(string msg)
    {
        Debug.Log("OnMove" + msg.ToString());
    }
    void OnLeave(string msg)
    {
        Debug.Log("OnLeave" + msg.ToString());
    }
}
