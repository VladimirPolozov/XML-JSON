using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

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
        public List<string> SynchronizeDirectories(string primaryDirectoryPath, string targetDirectoryPath)
        {
            DirectoryInfo primaryDirectoryInfo = new DirectoryInfo(primaryDirectoryPath);
            DirectoryInfo targetDirectoryInfo = new DirectoryInfo(targetDirectoryPath);
            List<string> resultEntries = new List<string>();
            Console.WriteLine(primaryDirectoryPath + " " + targetDirectoryPath);

            foreach (FileInfo primaryDirectoryFile in primaryDirectoryInfo.GetFiles())
            {
                FileInfo targetDirectoryFile = new FileInfo(Path.Combine(targetDirectoryInfo.FullName, primaryDirectoryFile.Name));

                if (!targetDirectoryFile.Exists)
                {
                    File.Copy(primaryDirectoryFile.FullName, targetDirectoryFile.FullName, true);
                    resultEntries.Add($"Файл {primaryDirectoryFile.Name} добавлен");
                }
                else if (targetDirectoryFile.LastWriteTime != primaryDirectoryFile.LastWriteTime)
                {
                    File.Copy(primaryDirectoryFile.FullName, targetDirectoryFile.FullName, true);
                    resultEntries.Add($"Файл {primaryDirectoryFile.Name} изменен");
                }                
            }

            foreach (FileInfo targetDirectoryFile in targetDirectoryInfo.GetFiles())
            {
                FileInfo primaryDirectoryFile = new FileInfo(Path.Combine(primaryDirectoryInfo.FullName, targetDirectoryFile.Name));

                if (!primaryDirectoryFile.Exists)
                {
                    targetDirectoryFile.Delete();
                    resultEntries.Add($"Файл {targetDirectoryFile.Name} удален");
                }
            }

            if (resultEntries.Count == 0)
            {
                resultEntries.Add("Директории идентичны");
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
