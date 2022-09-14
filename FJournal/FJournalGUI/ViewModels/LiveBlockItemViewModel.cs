using FJournalGUI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FJournalGUI.ViewModels
{
    public class LiveBlockItemViewModel : INotifyPropertyChanged
    {
        private DBLiveRecordModel liveRecordModel;
        public LiveBlockItemViewModel(DBLiveRecordModel dBLiveRecordModel)
        {
            this.Record = dBLiveRecordModel;
        }

        public DBLiveRecordModel Record { 
            get => this.liveRecordModel;
            set
            {
                this.liveRecordModel = value;
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
