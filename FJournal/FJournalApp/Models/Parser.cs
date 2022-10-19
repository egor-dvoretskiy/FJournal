using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace FJournalApp.Models
{
    public class Parser
    {
        public bool ParseDateTime(object? input, out SelectedDatesCollection dateTime)
        {
            if (input == null)
            {
                dateTime = new SelectedDatesCollection(null);
                return false;
            }

            dateTime = (SelectedDatesCollection) input;

            return true;
        }
    }
}
