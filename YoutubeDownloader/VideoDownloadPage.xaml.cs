using CGS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Threading;
using VideoLibrary;
using YoutubeExplode;

namespace YoutubeDownloader
{
    /// <summary>
    /// Interaction logic for VideoDownloadPage.xaml
    /// </summary>
    public partial class VideoDownloadPage : Page
    {
        private string videoURL;
        private object previousPage;
        private long progressVal;
        private bool finished;
        private Thread download;
        private String downloadPath;

        public VideoDownloadPage(object sender, string url, String path)
        {
            InitializeComponent();
            previousPage = sender;
            videoURL = url;
            downloadPath = path;
            downloadProgressBar.Minimum = 0;
            downloadProgressBar.Maximum = 100;
            doneButton.IsEnabled = false;

            download = new Thread(Download_Vid);

            download.Start();

        }

        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(previousPage);
        }

        private void Download_Vid()
        {
            finished = false;
            using (var service = Client.For(YouTube.Default))
            { 
                using (var video = service.GetVideo(videoURL))
                {
                    using (var outFile = File.OpenWrite(downloadPath + "/" + video.FullName))
                    {
                        using (var ps = new ProgressStream(outFile))
                        {
                            long streamLength = (long)video.StreamLength();
                            Stopwatch stopWatch = Stopwatch.StartNew();
                            ps.BytesMoved += (sender1, args) =>
                            {
                                var percentage = args.StreamLength * 100 / streamLength;
                                TimeSpan remaining = new TimeSpan();
                                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
                                {
                                    //update progress bar
                                    this.downloadProgressBar.Value = percentage;
                                    //update the overlayed percentage value
                                    this.downPercent.Content = percentage + "%";
                                    //update the elapsed time
                                    this.timeElapsed.Content = stopWatch.Elapsed;
                                    //update the time remaining.
                                    double timeRemaining = (long)(stopWatch.Elapsed.TotalMilliseconds / percentage) * (100 - percentage);
                                    remaining = TimeSpan.FromMilliseconds(timeRemaining);
                                    this.timeRemaining.Content = remaining;
                                }));
                            };

                            video.Stream().CopyTo(ps);
                        }
                    }
                }
            }

            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate ()
            {
                this.doneButton.IsEnabled = true;
            }));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            download.Abort();
            this.NavigationService.GoBack();
        }
    }
}
