using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Logger
{
    public class BaseLogger : ILogger
    {
        private string pathToFolder = ConfigurationManager.AppSettings["pathToFile"];

        private string logType = ConfigurationManager.AppSettings["typeOfFile"];

        private string level = ConfigurationManager.AppSettings["logLevel"];

        public void Log(string logMessage, LogLevel levelOfLog)
        {
            var logInfo = new Dictionary<string, string>();
            logInfo.Add("LogMessage", logMessage);
            logInfo.Add("LogLevel", levelOfLog.ToString());

            writeLog(logInfo);
        }

        public void Log(string logMessage, LogLevel levelOfLog, DateTime logDateAndTime)
        {
            var logInfo = new Dictionary<string, string>();
            logInfo.Add("LogMessage", logMessage);
            logInfo.Add("LogLevel", levelOfLog.ToString());
            logInfo.Add("LogDate", logDateAndTime.ToString());

            writeLog(logInfo);
        }

        public void Log(string logMessage, LogLevel levelOfLog, Type typeFromLogCalls)
        {
            var logInfo = new Dictionary<string, string>();
            logInfo.Add("LogMessage", logMessage);
            logInfo.Add("LogLevel", levelOfLog.ToString());
            logInfo.Add("Destination", typeFromLogCalls.ToString());

            writeLog(logInfo);
        }

        public void Log(string logMessage, 
            LogLevel levelOfLog, 
            DateTime logDateAndTime, 
            Type typeFromLogCalls)
        {
            var logInfo = new Dictionary<string, string>();
            logInfo.Add("LogMessage", logMessage);
            logInfo.Add("LogLevel", levelOfLog.ToString());
            logInfo.Add("LogDate", logDateAndTime.ToString());
            logInfo.Add("Destination", typeFromLogCalls.ToString());

            writeLog(logInfo);
        }

        private void writeInTxtFile(Dictionary<string, string> logInfo)
        {
            var pathToFile = string.Concat(pathToFolder, @"\LogInfo.txt");
            using (var writer = new StreamWriter(pathToFile, true))
            {
                foreach (var element in logInfo)
                    writer.Write($"{element.Key}: {element.Value} ");
                writer.Write("\r\n");
            }
        }

        private void writeInXmlFile(Dictionary<string, string> logInfo)
        {
            var pathToFile = string.Concat(pathToFolder, @"\LogInfo.xml");
            var xmlToDictionary = new XElement("log", logInfo.Select(
                x => new XAttribute(x.Key, x.Value)));

            var offset = Encoding.UTF8.GetBytes("</logs>").Length;
            using (var writer = new StreamWriter(File.OpenWrite(pathToFile)))
            {
                writer.BaseStream.Position = writer.BaseStream.Length - offset;
                writer.Write(String.Format("    {0}\r\n</logs>", xmlToDictionary));
            }
        }

        private void writeInJsonFile(Dictionary<string, string> logInfo)
        {

            var pathToFile = string.Concat(pathToFolder, @"\LogInfo.json");
            var serializeDictionary = JsonConvert.SerializeObject(logInfo);
            var offset = Encoding.UTF8.GetBytes("]").Length;
            using (var writer = new StreamWriter(File.OpenWrite(pathToFile)))
            {
                writer.BaseStream.Position = writer.BaseStream.Length - offset;
                writer.Write(String.Format("    {0},\r\n]", serializeDictionary));
            }
        }

        private void writeLog(Dictionary<string, string> logInfo)
        {
            switch (logType)
            {
                case "xml":
                    writeInXmlFile(logInfo);
                    break;
                case "json":
                    writeInJsonFile(logInfo);
                    break;
                default:
                    writeInTxtFile(logInfo);
                    break;
            }
        }
    }
}
