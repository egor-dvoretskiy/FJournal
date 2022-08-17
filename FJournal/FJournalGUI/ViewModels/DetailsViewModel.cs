using FJournalGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FJournalGUI.ViewModels
{
    public class DetailsViewModel
    {
        public DetailsViewModel(DBRecordModel? model, string title)
        {
            this.DBRecordViewModel = model;
            /*this.Title = title;*/
        }

        public DBRecordModel? DBRecordViewModel { get; private set; }

        public string Title { get; private set; } = "Details";
    }
}
