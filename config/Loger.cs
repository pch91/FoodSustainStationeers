using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;

namespace FoodSustain.Config
{
    internal static class Loger
    {
        private static ILogger logger = UnityEngine.Debug.unityLogger;

        public static void info(String msg)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);

            logger.Log(LogType.Log, "[ INFO ][ FoodSustain ] "+ stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + "." + stackFrame.GetMethod().Name + " " + msg);
        }

        public static void debug(String msg)
        {
            if (Configs.getconfigs<bool>("EnabledDebug")) {
                StackTrace stackTrace = new StackTrace();
                StackFrame stackFrame = stackTrace.GetFrame(1);

                logger.Log(LogType.Log, "[ DEBUG ][ FoodSustain ] " + stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + "." + stackFrame.GetMethod().Name + " " + msg);
            }
        }

        public static void error(String msg)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            logger.Log(LogType.Error, "[ Error ][ FoodSustain ] " + stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + " " + msg);
        }

        public static void warning(String msg)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            logger.Log(LogType.Warning, "[ Warning ][ FoodSustain ] " + stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + " " + msg);
        }

        public static void exception(Exception e)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            logger.Log(LogType.Exception, "[ Exception ][ FoodSustain ] " + stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + " " + e.StackTrace);
            Exception INNER = e.InnerException;
            while (INNER != null)
            {
                logger.LogFormat(LogType.Exception, "[ Inner Exception ][ FoodSustain ] " + stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + " " + INNER.StackTrace);
                INNER = INNER.InnerException;
            }
        }

        public static void infoFormat(String msg, params object[] args)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            logger.LogFormat(LogType.Log, "[ INFO ][ FoodSustain ] " + stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + " " + msg, args);
        }

        public static void errorFormat(String msg, params object[] args)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            logger.LogFormat(LogType.Error, "[ Error ][ FoodSustain ] " + stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + " " + msg, args);
        }

        public static void warningFormat(String msg, params object[] args)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            logger.LogFormat(LogType.Warning, "[ Warning ][ FoodSustain ] " + stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + " " + msg, args);
        }

        public static void exceptionFormat(Exception e, params object[] args)
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame stackFrame = stackTrace.GetFrame(1);
            logger.LogFormat(LogType.Exception, "[ Exception ][ FoodSustain ] " + stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + " " + e.StackTrace, args);
            Exception INNER = e.InnerException;
            while (INNER != null)
            {
                logger.LogFormat(LogType.Exception, "[ Inner Exception ][ FoodSustain ] " + stackFrame.GetMethod().DeclaringType + "." + stackFrame.GetMethod().Name + " " + INNER.StackTrace, args);
                INNER = INNER.InnerException;
            }
        }
    }
}
