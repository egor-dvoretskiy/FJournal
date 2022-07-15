using FJournalLib.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FJournalGUI.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public ApplicationViewModel()
        {
            this.Records = new ObservableCollection<DBRecordViewModel>()
            {
                new DBRecordViewModel(new DBRecord()
                {
                    LogSource = FJournalLib.Enums.LogSource.Inner,
                    CallerFilePath = "file path",
                    CallerLineNumber = 0,
                    CallerMemberName = "member name",
                    LogType = FJournalLib.Enums.LogType.Debug,
                    Message = "mesaga bez napryaga",
                    PrivateMemoryUsage = 213,
                    TimeStamp = DateTime.Now,
                    TotalCpuUsage = 33
                })
            };
        }

        public ObservableCollection<DBRecordViewModel> Records { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
