using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DebugTool
{
    private static bool isDebugMode = false;
    public static void ShowArrayLog(float[] data, string name)
    {
        if(!isDebugMode)
            return;
        string log = "";
        log += name + ": ";
        for (int i = 0; i < data.Length; i++)
        {
            log += data[i].ToString() + " ";
        }

        Debug.Log(log);
    }
    public static void ShowArrayLog(byte[] data, string name)
    {
        if(!isDebugMode)
            return;
        string log = "";
        log += name + ": ";
        for (int i = 0; i < data.Length; i++)
        {
            log += data[i] + " ";
        }

        Debug.Log(log);
    }

    public static void ShowArrayLog(int[] data, string name)
    {
        if(!isDebugMode)
            return;
        string log = "";
        log += name + ": ";
        for (int i = 0; i < data.Length; i++)
        {
            log += data[i] + " ";
        }

        Debug.Log(log);
    }

    public static void ShowArrayLog(byte[] data)
    {
        if(!isDebugMode)
            return;
        string log = "";
        for (int i = 0; i < data.Length; i++)
        {
            log += data[i] + " ";
        }

        Debug.Log(log);
    }

    public static void ShowArrayLog<T>(T[] data)
    {
        if(!isDebugMode)
            return;
        string log = "";
        for (int i = 0; i < data.Length; i++)
        {
            log += data[i] + " ";
        }

        Debug.Log(log);
    }
    
    public static void ShowArrayLog<T>(T[] data, string name)
    {
        if(!isDebugMode)
            return;
        string log = name;
        for (int i = 0; i < data.Length; i++)
        {
            log += data[i] + " ";
        }

        Debug.Log(log);
    }
}
