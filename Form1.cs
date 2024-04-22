using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace WindowsFormsLab
{
    public partial class Form1 : Form, ISynchronizationView
    {
        string[] folderPathInArray;
        string firstDirectoryPath;
        string secondDirectoryPath;

        public string GetFirstDirectoryPath() { return firstDirectoryPath; }
        public string GetSecondDirectoryPath() { return secondDirectoryPath; }

        public event EventHandler<EventArgs> SynchronizeFirstDirectoryWithSecondDirectoryEvent;
        public event EventHandler<EventArgs> SynchronizeSecondDirectoryWithFirstDirectoryEvent;

        public Form1()
        {
            InitializeComponent();
            SynchronizationPresenter programPresenter = new SynchronizationPresenter(this);
        }

        void ISynchronizationView.TrySynchronize(List<string> messages)
        {
            foreach (string message in messages)
            {
                LogChanges.Items.Add(message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void DirectoryOneLabel_Click(object sender, EventArgs e)
        {

        }

        private void ChoiceDirectoryOneButton_Click_1(object sender, EventArgs inputEvent)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                firstDirectoryPath = folderBrowserDialog.SelectedPath;
                folderPathInArray = firstDirectoryPath.Split(new char[] { '\\' });
                FirstDirectoryLabel.Text = folderPathInArray[folderPathInArray.Length - 1];
                UpdateSynchronizeButtonsLabel();
            }
        }

        private void ChoiceDirectoryTwoButton_Click_1(object sender, EventArgs inputEvent)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                secondDirectoryPath = folderBrowserDialog.SelectedPath;
                folderPathInArray = secondDirectoryPath.Split(new char[] { '\\' });
                DirectorySecondLabel.Text = folderPathInArray[folderPathInArray.Length - 1];
                UpdateSynchronizeButtonsLabel();
            }

        }

        private void UpdateSynchronizeButtonsLabel()
        {
            if (FirstDirectoryLabel.Text != "Директория №1" && DirectorySecondLabel.Text != "Директория №2")
            {
                SynchronizeFirstDirectoryWithSecondDirectoryButton.Text = $"Обновить {FirstDirectoryLabel.Text} до соответствия с {DirectorySecondLabel.Text}";
                SynchronizeSecondDirectoryWithFirstDirectoryButton.Text = $"Обновить {DirectorySecondLabel.Text} до соответствия с {FirstDirectoryLabel.Text}";
            }
        }

        private void SynchronizeFirstDirectoryWithSecondDirectoryButton_Click(object sender, EventArgs inputEvent)
        {
            if (FirstDirectoryLabel.Text != "Директория №1" && DirectorySecondLabel.Text != "Директория №2")
            {
                SynchronizeFirstDirectoryWithSecondDirectoryEvent(sender, inputEvent);
            }
        }

        private void SynchronizeSecondDirectoryWithFirstDirectoryButton_Click(object sender, EventArgs inputEvent)
        {
            if (FirstDirectoryLabel.Text != "Директория №1" && DirectorySecondLabel.Text != "Директория №2")
            {
                SynchronizeSecondDirectoryWithFirstDirectoryEvent(sender, inputEvent);
            }
        }
    }
}
