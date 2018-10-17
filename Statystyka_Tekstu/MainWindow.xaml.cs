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
using Microsoft.Win32;

namespace Statystyka_Tekstu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filepath;

        public MainWindow()
        {
            InitializeComponent();
            StartButton.IsEnabled = false;
        }

        private void Readfile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Text files (*.txt)|*.txt",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
            };

            if (openFileDialog.ShowDialog() == true)
            {
                filepath = openFileDialog.FileName;
                StartButton.IsEnabled = true;
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string fulltext = File.ReadAllText(filepath);
            StatOutput.Text = "Długość tekstu " + fulltext.Length + " znaków";

            int maxchar = 0;

            foreach (char c in fulltext)
            {
                if (c > maxchar) maxchar = c;
            }

            StatOutput.Text += Environment.NewLine + "Kod najwyższego znaku " + maxchar;
            if (maxchar > 255) StatOutput.Text += " kodowanie UTF-8";

            char[] chars=new char[maxchar+1];

            foreach (char c in fulltext)
            {
                chars[c]++;
            }

            var dictionary = new Dictionary<int, int>();

            for (int i=0;i <= maxchar;i++)
            {
                if(chars[i]>0)
                    dictionary.Add(i,chars[i]);
            }

            var items = from pair in dictionary
                orderby pair.Value descending 
                select pair;

            foreach (KeyValuePair<int, int> pair in items)
            {
                if(pair.Key==10)
                    StatOutput.Text += Environment.NewLine + "[" + pair.Key + "] '" + "NL" +
                                   "' wystąpień: " + pair.Value;
                else if(pair.Key==13)
                    StatOutput.Text += Environment.NewLine + "[" + pair.Key + "] '" + "CR" +
                                       "' wystąpień: " + pair.Value;

                else if(pair.Key==32)
                    StatOutput.Text += Environment.NewLine + "[" + pair.Key + "] '" + "SPACE" +
                                       "' wystąpień: " + pair.Value;
                else
                StatOutput.Text += Environment.NewLine + "[" + pair.Key + "] '" + Convert.ToChar(pair.Key) +
                                   "' wystąpień: " + pair.Value;
            }


        }
    }
}
