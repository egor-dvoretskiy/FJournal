using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FJournalApp.Models
{
    internal class DBLiveRecordModel :INotifyPropertyChanged
    {
        private DateTime timeStamp;

        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set 
            { 
                timeStamp = value;
                OnPropertyChanged("TimeStamp");
            }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set
            {
                message = value;
                OnPropertyChanged("Message");
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
