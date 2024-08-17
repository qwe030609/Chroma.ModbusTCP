using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Collections.Generic;

namespace Chroma.FuelCell.GatewayConnector.Model
{
    public static class LogExtensions
    {
        // Log List Data
        private static List<string> LogListData = new List<string>();

        /// <summary>
        /// Create Log with the given string 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public static void CreateLog(string log)
        {
            DateTime now = DateTime.Now;
            string tmpStr = ">" + now.ToLongTimeString() + ": " + log;
            LogListData.Add(tmpStr);
        }

        public static List<string> GetLogger()
        {
            return LogListData;
        }

        public static void ClearLogger()
        {
            LogListData.Clear();
        }
    }
}
