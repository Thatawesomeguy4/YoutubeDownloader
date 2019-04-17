using MediaToolkit;
using MediaToolkit.Model;
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
            var inputFile = new MediaFile { Filename = fileName };
            var outputFile = new MediaFile { Filename = $"{fileName}.mp3" };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                engine.Convert(inputFile, outputFile);
            }

            //done, go back.
            NavigationService.GoBack();
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
