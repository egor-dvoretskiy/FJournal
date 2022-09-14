using FJournalGUI.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FJournalGUI.ViewModels
{
    public class LiveBlockViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<DBLiveRecordModel> Records { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
