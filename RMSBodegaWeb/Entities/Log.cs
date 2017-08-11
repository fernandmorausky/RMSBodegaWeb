using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RMSBodegaWeb.Entities
{
   public class Log
   {
      public static void writeLog(String isMsg, EventLogEntryType intTypeMsg)
      {
            Logging.FileLogger Log = new Logging.FileLogger(System.Configuration.ConfigurationManager.AppSettings["pathLog"]);
            try
            {
                if (intTypeMsg == EventLogEntryType.Error)
                {
                    isMsg = "ERROR:" + isMsg;
                }

                if (intTypeMsg == EventLogEntryType.Error)
                {
                    Log.WriteLog(isMsg, intTypeMsg);
                }
                else
                {
                    Log.WriteLog(isMsg, intTypeMsg);
                }
            }
            catch (Exception)
            {
                //Nothing to do
            }
            finally
            {
                Log = null;
            }


        }
   }
}
