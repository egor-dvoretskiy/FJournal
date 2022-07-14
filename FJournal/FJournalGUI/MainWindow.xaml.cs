using FJournalLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FJournalGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Journal _journal;

        private readonly Timer _writeToMongoTimer;

        private readonly int _mongoTimerInterval = 1;

        public MainWindow()
        {
            InitializeComponent();

            this._journal = new FJournalLib.Journal();

            this._writeToMongoTimer = new Timer();
            this._writeToMongoTimer.Elapsed += _writeToMongoTimer_Elapsed;
            this._writeToMongoTimer.Interval = this._mongoTimerInterval;

            this._writeToMongoTimer.Start();
        }

        private void _writeToMongoTimer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            this._journal.Note(new FJournalLib.Models.TRecord()
            {
                Message = $"Hello from FJournal GUI, {Guid.NewGuid()}",
                LogType = FJournalLib.Enums.LogType.Info
            });
        }
    }
}
