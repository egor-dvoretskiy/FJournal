using FJournalLib;
using FJournalLib.Extensions;
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
using FJournalGUI.Models;
using FJournalGUI.Models.Filter;

namespace FJournalGUI.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private readonly IRepository<DBRecord> _mongoRecordRepository;

        public ApplicationViewModel()
        {
            this._mongoRecordRepository = new MongoRecordRepository();
            this.FilterSettingsModel = new FilterSettingModel();
        }

        public ObservableCollection<DBRecordModel> Records { get; set; }

        public ObservableCollection<DBLiveRecordModel> LiveRecords { get; set; }

        public FilterSettingModel FilterSettingsModel { get; private set; }

        public double Elapsed { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void UpdateFilterSettingsViewModel(FilterSettingModel filterSettingsViewModel)
        {
            this.FilterSettingsModel = filterSettingsViewModel;
            this.UpdateRecords();
        }

        private void UpdateRecords()
        {
            // TODO FILTER CLASS.

            IEnumerable<DBRecordModel> records = this.EvaluateTime(() => this.GetRecordsFromDBAccordingToFilter(this.FilterSettingsModel), out double EvaluatedTime);

            this.Elapsed = EvaluatedTime;
            this.Records = new ObservableCollection<DBRecordModel>(records);
        }

        private IEnumerable<DBRecordModel> GetRecordsFromDBAccordingToFilter(FilterSettingModel filter)
        {
            IEnumerable<DBRecordModel> records;

            if (filter.DateTimes.Count() > 0)
            {
                records = this._mongoRecordRepository
                    .GetRecordsByDateCollection(filter.DateTimes.ToList(), filter.AmountOfRecordsToDisplay)
                    .Where(x => x.Message.Contains(filter.MessageSpan))
                    .Select(x => new DBRecordModel(x));
            }
            else
            {
                records = this._mongoRecordRepository
                    .GetRecordsByAmount(filter.AmountOfRecordsToDisplay)
                    .Where(x => x.Message.Contains(filter.MessageSpan))
                    .Select(x => new DBRecordModel(x));
            }

            return records;
        }
    }
}
