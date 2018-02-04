using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using log4net;
using log4net.Core;

namespace LogLib
{
    public sealed class Logger
    {
        private static ILog log;

        private static ILog log_debug;

        private static readonly ConcurrentQueue<LogException> _cq = new ConcurrentQueue<LogException>();

        private static readonly ConcurrentQueue<String> _cdebug = new ConcurrentQueue<String>();

        private static ManualResetEvent _mre = new ManualResetEvent(false);

        private static AutoResetEvent _are = new AutoResetEvent(false);

        private Logger()
        {
            
        }

        public static void Init()
        {
            log4net.Repository.ILoggerRepository repository= LogManager.CreateRepository("NetCore");
            log4net.Config.XmlConfigurator.ConfigureAndWatch(repository, new FileInfo(Path.Combine(AppContext.BaseDirectory,"Log4Net.config")));
            log = LogManager.GetLogger(repository.Name, "FileLog");
            log_debug = LogManager.GetLogger(repository.Name, "RollingLog");
            Task.Run(new Action(WriteToFile));
            Task.Run(new Action(DebugToFile));
        }

        #region 日志打印
        public static void Warn(Exception e)
        {
            LogException ex = new LogException("warn", e);
            _cq.Enqueue(ex);
            _mre.Set();
        }

        public static void Error(Exception e)
        {
            LogException ex = new LogException("error", e);
            _cq.Enqueue(ex);
            _mre.Set();
        }

        private static void WriteToFile()
        {
            LogException ex=null;
            Exception e = null;
            while (true)
            {
                _mre.WaitOne();
                while (_cq.Count > 0 && _cq.TryDequeue(out ex))
                {
                    e = ex.InnerException;
                    while(null != e){
                        switch (ex.Message)
                        {
                            case "info":
                                log.Info(e.Message);
                                break;
                            case "error":
                                log.Error(e.Message);
                                break;
                            case "warn":
                                log.Warn(e.Message);
                                break;
                            case "fatal":
                                log.Debug(e.Message);
                                break;
                        }
                        e = e.InnerException;
                    }
                }
                _mre.Reset();
            }
        }
        #endregion

        #region 调试打印 单独调试文件

        public static void Debug(String s)
        {
            _cdebug.Enqueue(s);
            _are.Set();
        }

        private static void DebugToFile()
        {
            string msg = string.Empty;
            while (true)
            {
                _are.WaitOne();
                while (_cdebug.Count > 0 && _cdebug.TryDequeue(out msg))
                {
                    log_debug.Debug(msg);
                }
                _are.Reset();
            }
        }
        #endregion
    }
}
