using FJournalApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FJournalApp.ViewModels
{
    internal class DetailsViewModel : DependencyObject, INotifyPropertyChanged
    {
        private DBRecordModel _record;

        public event PropertyChangedEventHandler? PropertyChanged;

        public DetailsViewModel()
        {
        }

        public DBRecordModel Record
        {
            get => this._record;
            set
            {
                if (this._record == null)
                    this._record = value;
            }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title",
            typeof(string),
            typeof(DetailsViewModel),
            new PropertyMetadata(string.Empty)
        );

        public string Title
        {
            get => "Details";
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
