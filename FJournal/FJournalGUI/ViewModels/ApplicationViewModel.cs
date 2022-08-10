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

namespace FJournalGUI.ViewModels
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        private readonly MongoRecordRepository _mongoRecordRepository;

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
            this.FilterSettingsViewModel.AmountOfRecordsToDisplay = filterSettingsViewModel.AmountOfRecordsToDisplay;

            this.UpdateRecords();
        }

        private void UpdateRecords()
        {
            var recordsFromDatabase = this._mongoRecordRepository.GetRecordsByAmount(this.FilterSettingsViewModel.AmountOfRecordsToDisplay).Select(x => new DBRecordViewModel(x));
            this.Records = new ObservableCollection<DBRecordViewModel>(recordsFromDatabase);    
        }
    }
}
