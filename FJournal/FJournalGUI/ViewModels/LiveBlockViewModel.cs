using FJournalGUI.Models;
using FJournalGUI.Views;
using FJournalLib.Interfaces;
using FJournalLib.Models;
using FJournalLib.Repositories;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Controls;
using System.Windows.Threading;

namespace FJournalGUI.ViewModels
{
    public class LiveBlockViewModel : INotifyPropertyChanged
    {
        private readonly IJournalRepositoryBackground _mongoRecordRepository;
        private readonly System.Timers.Timer _timer;

        private double _timerInterval = 100;

        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        public LiveBlockViewModel()
        {
            this._mongoRecordRepository = new MongoRecordRepository();

            this._timer = new System.Timers.Timer();
            this._timer.Elapsed += _timer_Elapsed;
            this._timer.AutoReset = false;
            this._timer.Interval = this._timerInterval;
            this._timer.Enabled = false;
        }

        public delegate void LiveRecordsHandler(IEnumerable<DBLiveRecordModel> dBLiveRecordModel);

        public event PropertyChangedEventHandler? PropertyChanged;

        public event LiveRecordsHandler LiveRecordsOnAir;

        public double TimerInterval
        {
            get => this._timerInterval;
            set
            {
                if (value > 0)
                {
                    this._timerInterval = value;
                    this._timer.Interval = value;
                }
            }
        }

        public void Enable()
        {
            this._timer.Enabled = true;
        }

        public void Disable()
        {
            if (this.cancellationTokenSource != null)
            {
                this.cancellationTokenSource.Cancel();
                this.cancellationTokenSource.Dispose();
            }

            this._timer.Enabled = false;
        }

        private void _timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            this.cancellationTokenSource = new CancellationTokenSource();
            this.cancellationToken = this.cancellationTokenSource.Token;
            try
            {
                var collection = this._mongoRecordRepository.ObserveChanges(this.cancellationToken);
                this.LiveRecordsOnAir?.Invoke(collection
                    .Where(x => !string.IsNullOrEmpty(x.Message))
                    .Select(x => new DBLiveRecordModel() { Message = x.Message, TimeStamp = x.TimeStamp }));

            }
            catch (OperationCanceledException) { }

            this.cancellationTokenSource.Dispose();

            this._timer.Enabled = true;
        }
    }
}
