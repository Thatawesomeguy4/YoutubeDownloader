using System;
using System.Collections.Generic;
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
using System.Windows.Forms;
using System.IO;

namespace YoutubeDownloader
{
    /// <summary>
    /// Interaction logic for EnterVideoURLPage.xaml
    /// </summary>
    public partial class EnterVideoURLPage : Page
    {
        public String videoURL;
        private String downloadPath;

        public EnterVideoURLPage()
        {
            InitializeComponent();
            videoURL = "";
            downloadPath = "C:/video.mp4";
        }

        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            videoURL = videoURLText.ToString();
            VideoDownloadPage download = new VideoDownloadPage(this, videoURL, downloadPath);
            this.NavigationService.Navigate(download);
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            //old navigate code
            VideoInfoPage videoInfo = new VideoInfoPage(videoURL);
            this.NavigationService.Navigate(videoInfo);

            //listBox.Items.Add(videoInfo);
        }

        private void FileSelectButton_Click(object sender, RoutedEventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {

                    downloadPath = fbd.SelectedPath;
                    downloadButton.IsEnabled = true;

                }
            }
        }

        private void VideoURLText_GotFocus(object sender, RoutedEventArgs e)
        {

            videoURLText.SelectionStart = 0;
            videoURLText.SelectAll();

        }
    }
}
