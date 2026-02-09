using System.Text;
using UnityEngine;

namespace ContactGloveSDK
{
    public static class JsonHelper
    {
        public static string Serialize<T>(T obj)
        {
            return JsonUtility.ToJson(obj);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonUtility.FromJson<T>(json);
        }
        
        public static T Deserialize<T>(byte[] utf8Bytes)
        {
            string json = Encoding.UTF8.GetString(utf8Bytes);
            Debug.Log($"json: {json}");
            return JsonUtility.FromJson<T>(json);
        }
    }

}