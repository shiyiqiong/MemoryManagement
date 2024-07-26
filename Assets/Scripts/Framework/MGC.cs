using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MGC : MonoBehaviour
{
    public MData obj;

    private List<string> listStr;

    public void ClickGC()
    {
        GC.Collect();
        Debug.Log("GC");
        
    }

    public void ClickDelete()
    {
        listStr = obj.listData;
        Destroy(obj.gameObject);
        obj = null;
        Debug.Log("ClickDelete");
    }

}
