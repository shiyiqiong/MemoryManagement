using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MData : MonoBehaviour
{
    public List<string> listData;

    void Start()
    {

        listData = new List<string>();
        for (int i = 0; i < 10000; i++)
        {
            listData.Add("test");
        }
    }


    void OnDestroy()
    {
        listData.Clear();
        listData = null;
        Debug.Log("OnDestroy");
    }
}
