using FJournalGUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FJournalGUI.Views
{
    /// <summary>
    /// Interaction logic for LiveBlockItem.xaml
    /// </summary>
    public partial class LiveBlockItem : UserControl
    {
        public LiveBlockItem(DBLiveRecordModel liveBlockItemModel)
        {
            InitializeComponent();

            this.TextBlock_LiveWindowTimestamp.Text = liveBlockItemModel.TimeStamp.ToString("G");
            this.TextBlock_LiveWindowMessage.Text = liveBlockItemModel.Message;
        }
    }
}
