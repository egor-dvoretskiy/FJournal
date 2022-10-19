using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FJournalApp.Models
{
    public class FilterSettingsModel
    {
        public int AmountOfRecordsToDisplay { get; set; }

        public SelectedDatesCollection DateTimes { get; set; }

        public string MessageSpan { get; set; }

        public FilterSettingsModel() => this.Reset();

        public void Reset()
        {
            this.AmountOfRecordsToDisplay = 200;
            this.DateTimes = new SelectedDatesCollection(null);
            this.MessageSpan = string.Empty;
        }
    }
}
