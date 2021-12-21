using LibVLCSharp.Shared;
using m3uParser;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Media = LibVLCSharp.Shared.Media;
using MediaPlayer = LibVLCSharp.Shared.MediaPlayer;

namespace IPTVEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Channel> Channels;
        public readonly DependencyProperty ChannelProperty;

        public Channel Channel
        {
            get { return (Channel)GetValue(ChannelProperty); }
            set { SetValue(ChannelProperty, value); }
        }

        private LibVLC libvlc;
        private MediaPlayer mediaplayer;

        public MainWindow()
        {
            ChannelProperty = DependencyProperty.Register("Channel", typeof(Channel), typeof(MainWindow));

            InitializeComponent();
            DataContext = this;

            Channels = new ObservableCollection<Channel>();

            channelsGrid.ItemsSource = Channels;

        }

        private void DataGridColumnHeader_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DataGridRow_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {

        }

        private async void DataGridRow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            DataGridRow dataGridRow = sender as DataGridRow;
            var channel = dataGridRow.Item as Channel;

            await Play(channel.Url);
        }

        private void OpenCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void OpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            string playlist = "";
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "Playlist"; // Default file name
            dialog.DefaultExt = ".m3u"; // Default file extension
            dialog.Filter = "Text documents (.m3u)|*.m3u"; // Filter files by extension

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                playlist = dialog.FileName;
            }


            var simpleVodM3u = M3U.ParseFromFile(playlist);

            foreach(var vod in simpleVodM3u.Medias)
            {
                Channel c = new Channel(vod.MediaFile, vod.Title.InnerTitle, vod.Attributes.GroupTitle, vod.Attributes.TvgLogo);
                Channels.Add(c);
            }


            int i = 0;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public async Task Play(string url)
        {
            Core.Initialize();

            libvlc = new LibVLC(enableDebugLogs: true);

            if (mediaplayer == null)
            {
                mediaplayer = new MediaPlayer(libvlc);
                videoView.MediaPlayer = mediaplayer;
            }
            else if (mediaplayer != null && mediaplayer.IsPlaying)
            {
                mediaplayer.Stop();
            }

            //Fix for dumb error when slashes backwards
            url = url.Replace(@"\", "/");

            try
            {

                var media = new Media(libvlc, new Uri(url));
                await media.Parse(MediaParseOptions.ParseNetwork);
                //dynamic video = media.SubItems;
                mediaplayer.Play(media);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DataGridRow_GotFocus(object sender, RoutedEventArgs e)
        {
            DataGridRow dataGridRow = sender as DataGridRow;
            Channel = dataGridRow.Item as Channel;
        }
    }
}
