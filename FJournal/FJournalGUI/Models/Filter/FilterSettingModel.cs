using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FJournalGUI.Models.Filter
{
    public class FilterSettingModel
    {
        public int AmountOfRecordsToDisplay { get; set; } = 200;

        public int AmountOfLiveRecordsToDisplay { get; set; } = 24;

        public SelectedDatesCollection DateTimes { get; set; } = new SelectedDatesCollection(null);

        public string MessageSpan { get; set; } = string.Empty;
    }
}
