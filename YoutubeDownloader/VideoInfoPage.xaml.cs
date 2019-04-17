using System;
using System.Collections.Generic;
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
using YoutubeExplode;
using YoutubeExplode.Models;

namespace YoutubeDownloader
{
    /// <summary>
    /// Interaction logic for VideoInfoPage.xaml
    /// </summary>
    public partial class VideoInfoPage : Page
    {

        Video video;
        bool isDone;

        public VideoInfoPage(string url)
        {
            InitializeComponent();
            isDone = false;

            var id = YoutubeClient.ParseVideoId(url);
            YoutubeClient client = new YoutubeClient();
            Task.Run(async () => video = await client.GetVideoAsync(id)).Wait();

            string title = video.Title;
            TimeSpan duration = video.Duration;
            string author = video.Author;

            browser.Source = new Uri(video.GetEmbedUrl());

        }
    }
}
