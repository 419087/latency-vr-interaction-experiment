using System;
using System.Collections.Generic;

namespace ContactGloveSDK
{
    public enum GlovePriority
    {
        First = 0,
        Second = 1,
        Third = 2,
        Fourth = 3,
        Fifth = 4,
        Sixth = 5,
        None = -1,
    }

    public class DeviceStatus
    {
        public bool isMain;
        public int deviceType;
        public string name;
        public int color;
        public float ping;
        public bool isLeftConnected;
        public int leftBattery;
        public float leftPing;
        public bool isRightConnected;
        public int rightBattery;
        public float rightPing;
    }

    public static class DeviceStatusManager
    {
        private static readonly IDictionary<GlovePriority, string> _priorityToId = new Dictionary<GlovePriority, string>
        {
            { GlovePriority.First, null },
            { GlovePriority.Second, null },
            { GlovePriority.Third, null },
            { GlovePriority.Fourth, null },
            { GlovePriority.Fifth, null },
            { GlovePriority.Sixth, null },
        };

        private static readonly IDictionary<string, DeviceStatus> _idToStatus =
            new Dictionary<string, DeviceStatus>();
        
        private static readonly IDictionary<GlovePriority, IList<IGloveDataReceiver>> _handlers =
            new Dictionary<GlovePriority, IList<IGloveDataReceiver>>
            {
                { GlovePriority.First, new List<IGloveDataReceiver>() },
                { GlovePriority.Second, new List<IGloveDataReceiver>() },
                { GlovePriority.Third, new List<IGloveDataReceiver>() },
                { GlovePriority.Fourth, new List<IGloveDataReceiver>() },
                { GlovePriority.Fifth, new List<IGloveDataReceiver>() },
                { GlovePriority.Sixth, new List<IGloveDataReceiver>() },
            };

        public static bool RegisterId(string id)
        {
            if (id == null) return false;

            foreach (var (p, priorityId) in _priorityToId)
            {
                if (priorityId == id)
                {
                    return false;
                }
            }
            
            foreach (var (p, priorityId) in _priorityToId)
            {
                if (priorityId == null)
                {
                    _priorityToId[p] = id;
                    return true;
                }
            }
            
            return false;
        }
        
        public static void RegisterHandler(GlovePriority priority, IGloveDataReceiver handler)
        {
            _handlers[priority].Add(handler);
        }
        
        public static void UnregisterHandler(GlovePriority priority, IGloveDataReceiver handler)
        {
            _handlers[priority].Remove(handler);
        }
        
        public static void HandleById(string id, Action<IGloveDataReceiver> handler)
        {
            var priority = GetPriority(id);
            if (priority == GlovePriority.None) return;
            
            foreach (var h in _handlers[priority])
            {
                handler(h);
            }
        }
        
        private static IEnumerable<IGloveDataReceiver> GetHandlers(string id)
        {
            if (id == null)
            {
                yield break;
            }
            
            var priority = DeviceStatusManager.GetPriority(id);
            
            if (priority == GlovePriority.None)
            {
                yield break;
            }

            if (!_handlers.TryGetValue(priority, out var handlers)) yield break;
            
            foreach (var handler in handlers)
            {
                yield return handler;
            }
        }

        public static void SetStatus(string id, DeviceStatus status)
        {
            RegisterId(id);

            _idToStatus[id] = status;
        }

        public static DeviceStatus GetStatus(GlovePriority priority)
        {
            if (priority == GlovePriority.None) return null;

            var id = _priorityToId[priority];

            return id == null ? null : _idToStatus[id];
        }

        public static GlovePriority GetPriority(string id)
        {
            if (id == null) return GlovePriority.None;

            foreach (var (p, priorityId) in _priorityToId)
            {
                if (priorityId == id)
                {
                    return p;
                }
            }

            return GlovePriority.None;
        }
        
        public static string GetId(GlovePriority priority)
        {
            if (priority == GlovePriority.None) return null;

            return _priorityToId[priority];
        }
    }
}