using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ChatBackend.ChatBackend _backend;
        public MainWindow()
        {
            InitializeComponent();
            _backend = new ChatBackend.ChatBackend(this.DisplayMessage, this.DisplayFile);
        }

        public void DisplayMessage(ChatBackend.CompositeType composite)
        {
            string username = composite.Username ?? "";
            string message = composite.Message ?? "";
            textBoxChatPane.Text += (username + ": " + message + Environment.NewLine);
        }

        public void DisplayFile(ChatBackend.CompositeType composite)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            saveFileDialog.Title = $"Guardar archivo {composite.Filename}";
            if (saveFileDialog.ShowDialog() == true)
            {
                File.WriteAllBytes(saveFileDialog.FileName, composite.File);
            }
        }

        private void textBoxEntryField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                _backend.SendMessage(textBoxEntryField.Text);
                textBoxEntryField.Clear();
            }
        }        

        private void SendFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == true)
            {
                var fileStream = openFileDialog.OpenFile();
                var fileByteArray = ReadFully(fileStream);

                _backend.SendFile(openFileDialog.FileName, fileByteArray);
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
