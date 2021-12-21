using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IPTVEditor
{
    public class Channel : INotifyPropertyChanged
    {
        public Channel(string url, string title = "unknown channel", string group_title = "", string tvg_logo = "", string tvg_id = "", string tvg_country = "", string tvg_language = "")
        {
            Url = url;
            Title = title;
            Group = group_title;
            Logo = tvg_logo;
            Lang = tvg_language;
            Country = tvg_country;
            TvgID = tvg_id;
        }

        private string title = string.Empty;
        private string url = string.Empty;
        private string group = string.Empty;
        private string logo = string.Empty;
        private string lang = string.Empty;
        private string country = string.Empty;
        private string tvgId = string.Empty;

        public string Title {
            get => title;
            set => SetField(ref title, value);
        }

        public string Url {
            get => url;
            set => SetField(ref url, value);
        }

        public string Group {
            get => group;
            set => SetField(ref group, value);
        }

        public string Logo {
            get => logo;
            set => SetField(ref logo, value);
        }

        public string Lang {
            get => lang;
            set => SetField(ref lang, value);
        }

        public string Country {
            get => country;
            set => SetField(ref country, value);
        }

        public string TvgID {
            get => tvgId;
            set => SetField(ref tvgId, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString() => Title.ToString();

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}
