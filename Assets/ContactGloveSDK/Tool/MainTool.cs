using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ContactGloveSDK
{
    public static class MainTool
    {
        public static float MapValue(float value, float min, float max, float min2, float max2)
        {
            return (value - min) * (max2 - min2) / (max - min) + min2;
        }

        public static Quaternion ConvertOpenVRToUnity(Quaternion q)
        {
            return new Quaternion(-q.w, q.x, -q.z, q.y);

            // return new Quaternion(q.w, q.x, q.y, q.z);
        }

        public static Vector3 ConvertOpenVRToUnity(Vector3 v)
        {
            return new Vector3(-v.z, v.y, -v.x);
        }

        public static float MagnitudeOfQuaternion(Quaternion q)
        {
            return Mathf.Sqrt(q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w);
        }

        // get yaw value, then return the quaternion that reverts the yaw
        public static Quaternion RevertYaw(Quaternion q)
        {
            Vector3 v = q * Vector3.forward;
            float yaw = Mathf.Atan2(v.z, v.y) * Mathf.Rad2Deg;
            return Quaternion.AngleAxis(yaw, Vector3.up);
        }

        // averaging Strength: 0~1, 0 means closer to average, 1 means further from average
        public static int MovingAverage(ref Queue<int> q, int value, float averagingStrength, int maxSize = 10)
        {
            q.Enqueue(value);
            if (q.Count > maxSize)
                q.Dequeue();
            float sum = 0;
            foreach (var f in q)
                sum += f;
            return (int)Mathf.Lerp(sum / q.Count, value, averagingStrength);
        }

        // averaging Strength: 0~1, 0 means closer to average, 1 means further from average
        public static float MovingAverage(ref Queue<float> q, float value, float averagingStrength, int maxSize = 10)
        {
            q.Enqueue(value);
            if (q.Count > maxSize)
                q.Dequeue();
            float sum = 0;
            foreach (var f in q)
                sum += f;
            return Mathf.Lerp(sum / q.Count, value, averagingStrength);
        }

        public static float RCFilter(float previousVal, float currentVal,
            float rcFilterParam)
        {
            return (float)previousVal * (1f - rcFilterParam) +
                   (float)currentVal * rcFilterParam;
        }

        public static Transform GetChildrenByName(Transform parent, string name)
        {
            if (parent.name == name)
            {
                return parent;
            }

            // If no children exist, return null
            if (parent.childCount == 0)
            {
                return null;
            }

            // iterate through all children
            foreach (Transform ob in parent)
            {
                if (ob.name == name)
                {
                    return ob;
                }

                var child_t = GetChildrenByName(ob, name);
                if (child_t != null)
                {
                    return child_t;
                }
            }

            return null;
        }
        
        
    }
}