using FJournalGUI.Models.Filter;
using FJournalLib;
using FJournalLib.Models;
using FJournalLib.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FJournalLib.Interfaces;
using System.Diagnostics;

namespace FJournalGUI.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private readonly IRepository<DBRecord> _mongoRecordRepository;

        public ApplicationViewModel()
        {
            this._mongoRecordRepository = new MongoRecordRepository();

            this.FilterSettingsViewModel = new FilterSettingsViewModel();

            this.UpdateRecords();
        }

        public ObservableCollection<DBRecordViewModel> Records { get; set; }

        public FilterSettingsViewModel FilterSettingsViewModel { get; private set; }

        public double Elapsed { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void UpdateFilterSettingsViewModel(FilterSettingsViewModel filterSettingsViewModel)
        {
            this.FilterSettingsViewModel = filterSettingsViewModel;
            this.UpdateRecords();
        }

        private void UpdateRecords()
        {
            // TODO FILTER CLASS.

            IEnumerable<DBRecordViewModel> records;

            Stopwatch sw = Stopwatch.StartNew();

            if (this.FilterSettingsViewModel.DateTimes.Count() > 0)
            {
                records = this._mongoRecordRepository
                    .GetRecordsByDateCollection(this.FilterSettingsViewModel.DateTimes.ToList(), this.FilterSettingsViewModel.AmountOfRecordsToDisplay)
                    .Where(x => x.Message.Contains(this.FilterSettingsViewModel.MessageSpan))
                    .Select(x => new DBRecordViewModel(x));
            }
            else
            {
                records = this._mongoRecordRepository
                    .GetRecordsByAmount(this.FilterSettingsViewModel.AmountOfRecordsToDisplay)
                    .Where(x => x.Message.Contains(this.FilterSettingsViewModel.MessageSpan))
                    .Select(x => new DBRecordViewModel(x));
            }

            sw.Stop();
            this.Elapsed = sw.Elapsed.TotalMilliseconds;

            this.Records = new ObservableCollection<DBRecordViewModel>(records);    
        }
    }
}
