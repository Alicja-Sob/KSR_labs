using MediaPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WMPLib;
using MessageBox = System.Windows.MessageBox;

namespace zad7
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private AxAcroPDF1 axPdf;
        //private AxWindowsMediaPlayer axPlayer;

        private System.Windows.Forms.WebBrowser winFormsBrowser;
        private System.Windows.Forms.WebBrowser pdfWebViewer;
        private WindowsMediaPlayer vidPlayer;

        public MainWindow()
        {
            InitializeComponent();

            // strona www
            winFormsBrowser = new System.Windows.Forms.WebBrowser();
            winFormsBrowser.Dock = DockStyle.Fill;
            webHost.Child = winFormsBrowser;
            winFormsBrowser.Navigate("https://www.example.com/");

            //windows media player
            playerHost.Source = new Uri(@"C:\Users\sobal\Downloads\exampleWAV.wav"); // <-- replace with your WAV file
            playerHost.Play();

            //pdf file
            pdfWebViewer = new System.Windows.Forms.WebBrowser();
            pdfWebViewer.Dock = DockStyle.Fill;
            pdfHost.Child = pdfWebViewer;
           // pdfWebViewer.Navigate(new Uri("about:blank"));
            pdfWebViewer.Navigate(@"C:\Users\sobal\Downloads\lab2_rh_ip_command_cheatsheet_1214_jcs_print.pdf");
        }

        private void mediaPlayer_MediaOpened(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wav file playing");
        }

        private void mediaPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wav finished playing");
        }
    }
}

