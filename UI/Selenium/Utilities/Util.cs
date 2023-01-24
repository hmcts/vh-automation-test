using System;
using System.Linq;
using NLog;
using NLog.Layouts;
using NLog.Targets;

namespace UI.Utilities
{
    ///<summary>
    /// Class with general utilities function to generate random data
    ///</summary>
    public class Util
    {
      
        public static string RandomString(int stringLenth = 10)
        {
            Random rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, stringLenth)
             .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        public static string RandomAlphabet(int stringLenth = 10)
        {
            Random rnd = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return new string(Enumerable.Repeat(chars, stringLenth)
             .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        public static string GetLogFileName(string targetName)
        {
            string rtnVal = string.Empty;

            if (LogManager.Configuration != null && LogManager.Configuration.ConfiguredNamedTargets.Count != 0)
            {
                Target t = LogManager.Configuration.FindTargetByName(targetName);
                if (t != null && t is NLog.Targets.Wrappers.WrapperTargetBase)
                {
                    var list = LogManager.Configuration.AllTargets.ToList();
                    t = list.Find(x => x.Name == $"{targetName}_wrapped");
                    Layout layout = (t as FileTarget).FileName;
                    rtnVal = layout.Render(LogEventInfo.CreateNullEvent());
                }
            }
            return rtnVal;
        }
    }
}
