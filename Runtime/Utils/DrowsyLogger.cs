using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace LuckiusDev.Utils
{
    public static class DrowsyLogger
    {
        private static void DoLog(Action<string, Object> logFunction, string prefix, Object obj, params object[] msg)
        {
#if UNITY_EDITOR
            var name = ( obj ? obj.name : "NullObject" ).ToColor("lightblue");
            logFunction($"{prefix}[{name}]: {string.Join("; ", msg)}\n ", obj);
#endif
        }

        public static void LogDebug(this Object obj, params object[] msg)
        {
            DoLog(Debug.Log, "", obj, msg);
        }

        public static void LogError(this Object obj, params object[] msg)
        {
            DoLog(Debug.LogError, "X".ToColor("red"), obj, msg);
        }

        public static void LogWarning(this Object obj, params object[] msg)
        {
            DoLog(Debug.LogWarning, "⚠️".ToColor("yellow"), obj, msg);
        }

        public static void LogSuccess(this Object obj, params object[] msg)
        {
            DoLog(Debug.Log, "✔️".ToColor("green"), obj, msg);
        }

        public static void LogInfo(this Object obj, params object[] msg)
        {
            DoLog(Debug.Log, "ℹ️".ToColor("cyan"), obj, msg);
        }

        public static void LogTrace(this Object obj, params object[] msg)
        {
            DoLog(Debug.Log, "🔍".ToColor("grey"), obj, msg);
        }

        public static void LogCritical(this Object obj, params object[] msg)
        {
            DoLog(Debug.LogError, "💥".ToColor("magenta"), obj, msg);
        }
    }
}
