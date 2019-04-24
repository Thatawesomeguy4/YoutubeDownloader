using FFMpegCore;
using FFMpegCore.Enums;
using FFMpegCore.FFMPEG;
using MediaToolkit;
using MediaToolkit.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace YoutubeDownloader
{
    /// <summary>
    /// Interaction logic for AudioCopyPage.xaml
    /// </summary>
    public partial class AudioCopyPage : Page
    {
        private string fileName;

        public AudioCopyPage(string fileName)
        {
            this.fileName = fileName;
            InitializeComponent();
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            //user wants an audio copy, create the copy and close the window once done.
            var inputFile = fileName;
            var outputFile = fileName + ".mp3";
            audioProgress.Maximum = 100;

            //set FFMpegCore Binary
            FFMpegOptions.Configure(new FFMpegOptions { RootDirectory = "C:\\Program Files (x86)\\FFMPEG\\bin" });

            FFMpeg ffmpeg = new FFMpeg();
            ffmpeg.OnProgress += (percentage) => audioProgress.Value = percentage;

            Thread converter = new Thread(new ThreadStart(() => ffmpeg.ExtractAudio(VideoInfo.FromPath(inputFile), new FileInfo(outputFile))));
            converter.Start();

            //done, go back.
            NavigationService.GoBack();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
