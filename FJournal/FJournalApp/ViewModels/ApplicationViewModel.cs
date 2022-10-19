using EMonolithLib.Extensions.Timing;
using FJournalApp.Models;
using FJournalApp.ViewModels.Commands;
using FJournalLib.Interfaces;
using FJournalLib.Models;
using FJournalLib.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FJournalApp.ViewModels
{
    internal class ApplicationViewModel : INotifyPropertyChanged
    {
        private readonly IRepository<DBRecord> _mongoRecordRepository;
        private readonly System.Timers.Timer _timer;
        private readonly Random _random;
        private readonly Parser _parser;

        private RelayCommand applyFilterCommand;
        private RelayCommand resetFilterCommand;
        private double elapsedUnload;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ApplicationViewModel()
        {
            this._parser = new Parser();
            this._mongoRecordRepository = new MongoRecordRepository();
            this.FilterSettings = new FilterSettingsModel();
            this.LiveRecords = new ObservableCollection<DBLiveRecordModel>();

            this._random = new Random();
            this._timer = new System.Timers.Timer();
            this._timer.Elapsed += _timer_Elapsed;
            this._timer.AutoReset = true;
            this._timer.Interval = 1000;
            this._timer.Enabled = true;
        }

        private void _timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                
                this.LiveRecords.Add(new DBLiveRecordModel()
                {
                    Message = this._random.Next(0, 256).ToString(),
                    TimeStamp = DateTime.Now,
                });
                OnPropertyChanged("LiveRecords");
            }
            catch (NotSupportedException exc)
            {
                _ = exc;
            }
        }

        public RelayCommand ApplyFilterCommand
        {
            get
            {
                return this.applyFilterCommand ??
                    (this.applyFilterCommand = new RelayCommand(obj =>
                    {
                        if (this._parser.ParseDateTime(obj, out SelectedDatesCollection dateTime))
                        {
                            this.FilterSettings.DateTimes = dateTime;
                        }

                        this.UpdateRecords();
                        this.OnPropertyChanged("Records");
                    }));
            }
        }

        public RelayCommand ResetFilterCommand
        {
            get
            {
                return this.resetFilterCommand ??
                    (this.resetFilterCommand = new RelayCommand(obj =>
                    {
                        this.FilterSettings.Reset();
                        this.UpdateRecords();
                        this.OnPropertyChanged("Records");
                        this.OnPropertyChanged("FilterSettings");
                    }));
            }
        }

        public double ElapsedUnload 
        {
            get => this.elapsedUnload; 
            set
            {
                this.elapsedUnload = value;
                this.OnPropertyChanged("ElapsedUnload");
            }
        }

        public FilterSettingsModel FilterSettings { get; set; }

        public ObservableCollection<DBRecordModel> Records { get; set; }

        public ObservableCollection<DBLiveRecordModel> LiveRecords { get; set; }

        private void UpdateRecords()
        {
            IEnumerable<DBRecordModel> records = this.EvaluateTime(() => this.GetRecordsFromDBAccordingToFilter(this.FilterSettings), out double EvaluatedTime);

            this.ElapsedUnload = EvaluatedTime;
            this.Records = new ObservableCollection<DBRecordModel>(records);
        }

        private IEnumerable<DBRecordModel> GetRecordsFromDBAccordingToFilter(FilterSettingsModel filter)
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

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
