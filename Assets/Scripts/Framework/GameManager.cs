using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;
using System.IO;
using UnityEngine.Networking;

public class GameManager : MonoBehaviour
{
    public TextAsset luaScript;

    internal static LuaEnv luaEnv; //all lua behaviour shared one luaenv only!

    private LuaTable scriptScopeTable;

    private Action luaInit, luaLoad, luaRelease;

    AssetBundle abRes;

    List<string> listData = new List<string>();

    void Awake()
    {
        luaEnv = new LuaEnv(); //all lua behaviour shared one luaenv only!
    }


    IEnumerator Start()
    {

        string url = Application.streamingAssetsPath + "/res" ;
        UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return request.SendWebRequest();
        abRes = DownloadHandlerAssetBundle.GetContent(request);


        // 为每个脚本设置一个独立的脚本域，可一定程度上防止脚本间全局变量、函数冲突
        scriptScopeTable = luaEnv.NewTable();

        // 设置其元表的 __index, 使其能够访问全局变量
        using (LuaTable meta = luaEnv.NewTable())
        {
            meta.Set("__index", luaEnv.Global);
            scriptScopeTable.SetMetaTable(meta);
        }

    
        luaEnv.AddLoader(new LuaEnv.CustomLoader((ref string filepath) =>
        {
            filepath = filepath.Replace('.', '/') + ".lua";
            TextAsset file = abRes.LoadAsset<TextAsset>(filepath);
            if (file != null)
            {
                Debug.Log(filepath);
                var bytes = file.bytes;
                Resources.UnloadAsset(file);
                return bytes;
            }
            else
            {
                return null;
            }
        }));

        // 执行脚本
        luaEnv.DoString(luaScript.text, luaScript.name, scriptScopeTable);
        scriptScopeTable.Get("init", out luaInit);
        scriptScopeTable.Get("load", out luaLoad);
        scriptScopeTable.Get("release", out luaRelease);



        Action luaStart = scriptScopeTable.Get<Action>("start");
        if (luaStart != null)
        {
            luaStart();
        }

        Init();

    }

    void Init()
    {
        if (luaInit != null)
        {
            luaInit();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadData()
    {
        if (luaLoad != null)
        {
            luaLoad();
        }
    }

    public void ReleaseData()
    {
        if (luaRelease != null)
        {
            luaRelease();
        }
        
    }
    
}
