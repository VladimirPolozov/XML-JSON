using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using NLog;

namespace WindowsFormsLab
{
    interface ISynchronizationView {
        string GetFirstDirectoryPath();
        string GetSecondDirectoryPath();

        void TrySynchronize(List<string> message);

        event EventHandler<EventArgs> SynchronizeFirstDirectoryWithSecondDirectoryEvent;
        event EventHandler<EventArgs> SynchronizeSecondDirectoryWithFirstDirectoryEvent;
    }

    class SynchronizationModel {
        [Obsolete]
        public List<string> SynchronizeDirectories(string primaryDirectoryPath, string targetDirectoryPath)
        {   
            Logger logger = LogManager.GetCurrentClassLogger();

            DirectoryInfo primaryDirectoryInfo = new DirectoryInfo(primaryDirectoryPath);
            DirectoryInfo targetDirectoryInfo = new DirectoryInfo(targetDirectoryPath);
            List<string> resultEntries = new List<string>();

            logger.Info($"Синхронизация директории {targetDirectoryPath} до соответствия с директорией {primaryDirectoryPath}Результат:");

            foreach (FileInfo primaryDirectoryFile in primaryDirectoryInfo.GetFiles())
            {
                FileInfo targetDirectoryFile = new FileInfo(Path.Combine(targetDirectoryInfo.FullName, primaryDirectoryFile.Name));

                if (!targetDirectoryFile.Exists)
                {
                    File.Copy(primaryDirectoryFile.FullName, targetDirectoryFile.FullName, true);
                    resultEntries.Add($"Файл {primaryDirectoryFile.Name} добавлен");
                    logger.Info($"Файл {primaryDirectoryFile.Name} добавлен в директорию {targetDirectoryPath}");
                }
                else if (targetDirectoryFile.LastWriteTime != primaryDirectoryFile.LastWriteTime)
                {
                    File.Copy(primaryDirectoryFile.FullName, targetDirectoryFile.FullName, true);
                    resultEntries.Add($"Файл {primaryDirectoryFile.Name} изменен");
                    logger.Info($"Файл {primaryDirectoryFile.Name} в директории {targetDirectoryPath} изменен");
                }
            }

            foreach (FileInfo targetDirectoryFile in targetDirectoryInfo.GetFiles())
            {
                FileInfo primaryDirectoryFile = new FileInfo(Path.Combine(primaryDirectoryInfo.FullName, targetDirectoryFile.Name));

                if (!primaryDirectoryFile.Exists)
                {
                    targetDirectoryFile.Delete();
                    resultEntries.Add($"Файл {targetDirectoryFile.Name} удален");
                    logger.Info($"Файл {targetDirectoryFile.Name} удален из директории {targetDirectoryPath}");
                }
            }

            if (resultEntries.Count == 0)
            {
                resultEntries.Add("Директории идентичны");
                logger.Info($"Директории идентичны");
            }

            return resultEntries;
        }
    }

    class SynchronizationPresenter
    {
        private ISynchronizationView mainView;
        private SynchronizationModel model;

        public SynchronizationPresenter(ISynchronizationView inputView)
        {
            mainView = inputView;
            model = new SynchronizationModel();

            mainView.SynchronizeFirstDirectoryWithSecondDirectoryEvent += new EventHandler<EventArgs>(SynchronizeFirstDirectoryWithSecondDirectory);
            mainView.SynchronizeSecondDirectoryWithFirstDirectoryEvent += new EventHandler<EventArgs>(SynchronizeSecondDirectoryWithFirstDirectory);
        }

        private void SynchronizeFirstDirectoryWithSecondDirectory(object sender, EventArgs inputEvent)
        {
            mainView.TrySynchronize(model.SynchronizeDirectories(mainView.GetSecondDirectoryPath(), mainView.GetFirstDirectoryPath()));
        }

        private void SynchronizeSecondDirectoryWithFirstDirectory(object sender, EventArgs inputEvent)
        {
            mainView.TrySynchronize(model.SynchronizeDirectories(mainView.GetFirstDirectoryPath(), mainView.GetSecondDirectoryPath()));
        }
    }
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
