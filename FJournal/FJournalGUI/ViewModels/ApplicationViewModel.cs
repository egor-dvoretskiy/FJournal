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

namespace FJournalGUI.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private readonly IRepository<DBRecord> _mongoRecordRepository;

        // TODO CACHING RECORDS

        public ApplicationViewModel()
        {
            this._mongoRecordRepository = new MongoRecordRepository();

            this.FilterSettingsViewModel = new FilterSettingsViewModel();

            this.UpdateRecords();
        }

        public ObservableCollection<DBRecordViewModel> Records { get; set; }

        public FilterSettingsViewModel FilterSettingsViewModel { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void UpdateFilterSettingsViewModel(FilterSettingsViewModel filterSettingsViewModel)
        {
            this.FilterSettingsViewModel = filterSettingsViewModel;
            this.UpdateRecords();
        }

        private void UpdateRecords()
        {
            // TODO FILTER CLASS.

            /*IEnumerable<DBRecordViewModel> records;

            if (this.CachedRecords == null)
            {
                this.CachedRecords = new List<DBRecordViewModel>();
            }

            if (this.FilterSettingsViewModel.AmountOfRecordsToDisplay <= this.CachedRecords.Count)
            {
                records = this.CachedRecords.TakeLast(this.FilterSettingsViewModel.AmountOfRecordsToDisplay);
            }
            else
            {

            }*/

            var recordsFromDatabase = this._mongoRecordRepository
                .GetRecordsByAmount(this.FilterSettingsViewModel.AmountOfRecordsToDisplay)
                .Select(x => new DBRecordViewModel(x))
                .Where(x => x.Message.Contains(this.FilterSettingsViewModel.MessageSpan));

            this.Records = new ObservableCollection<DBRecordViewModel>(recordsFromDatabase);    
        }
    }
}
