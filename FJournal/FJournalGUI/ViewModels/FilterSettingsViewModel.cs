using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FJournalGUI.ViewModels
{
    public class FilterSettingsViewModel
    {
        public int AmountOfRecordsToDisplay { get; set; } = 333;

        public SelectedDatesCollection DateTimes { get; set; } = new SelectedDatesCollection(null);

        public string MessageSpan { get; set; } = string.Empty;
    }
}
