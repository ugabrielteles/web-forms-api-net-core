using System;
using System.IO;
using System.Web;

namespace Order.Frontend.WebForms.Utils
{

    public static class LogUtil
    {
        private static string logFilePath = HttpContext.Current.Server.MapPath("~/App_Data/ErrorLogs.txt");

        public static void LogError(Exception ex)
        {
            try
            {
                string errorMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Error: {ex.Message}{Environment.NewLine}";
                errorMessage += $"StackTrace: {ex.StackTrace}{Environment.NewLine}";

                File.AppendAllText(logFilePath, errorMessage);
            }
            catch (Exception logException)
            {
                Console.WriteLine("Failed to log error: " + logException.Message);
            }
        }

        public static void LogMessage(string message)
        {
            try
            {
                string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - Message: {message}{Environment.NewLine}";

                File.AppendAllText(logFilePath, logMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to log message: " + ex.Message);
            }
        }
    }
}