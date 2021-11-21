using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BepInEx.Logging;

namespace COM3D2.AlternativeEditMenuFilter
{
    internal class Log
    {
        internal static ManualLogSource Logger
        {
            get
            {
                return AlternateEditMenuFilterPlugin.Instance.Logger;
            }
        }

        internal static bool enableVerbose
        {
            get
            {
                return false;
            }
        }

        internal static void LogInfo(string message, params object[] args)
        {
            Logger.LogInfo(string.Format(message, args));
        }

        internal static void LogError(string message, params object[] args)
        {
            Logger.LogError(string.Format(message, args));
        }

        internal static void LogVerbose(string message, params object[] args)
        {
            if (!enableVerbose) return;
            Logger.LogInfo(string.Format(message, args));
        }
    }
}
