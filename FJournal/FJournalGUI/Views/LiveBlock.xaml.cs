using FJournalGUI.Models;
using FJournalGUI.ViewModels;
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
    /// Interaction logic for LiveBlock.xaml
    /// </summary>
    public partial class LiveBlock : UserControl
    {
        private readonly LiveBlockViewModel _liveBlockViewModel;
        private int itemsCountLimit = 50;
        public LiveBlock()
        {
            InitializeComponent();

            this._liveBlockViewModel = new LiveBlockViewModel();
            this.DataContext = this._liveBlockViewModel;

            this._liveBlockViewModel.LiveRecordsOnAir += _liveBlockViewModel_LiveRecordsOnAir;
        }

        public int ItemsCountLimit 
        {
            get => this.itemsCountLimit;
            set
            {
                if (value > 0)
                    this.itemsCountLimit = value;
            }
        }

        public void EnableLiveBlock()
        {
            this._liveBlockViewModel.Enable();
        }

        public void DisableLiveBlock()
        {
            this._liveBlockViewModel.Disable();
        }

        public void AddItem(DBLiveRecordModel liveBlockItemModel)
        {
            Dispatcher.BeginInvoke(() => 
            {
                this.DeleteExtraItemsIfNecessary();
                this.StackPanel_Records.Children.Add(new LiveBlockItem(liveBlockItemModel));
            });            
        }

        private void DeleteExtraItemsIfNecessary()
        {
            while (this.StackPanel_Records.Children.Count >= itemsCountLimit)
            {
                this.StackPanel_Records.Children.RemoveAt(0);
            }
        }

        private void _liveBlockViewModel_LiveRecordsOnAir(IEnumerable<DBLiveRecordModel> dBLiveRecordModel)
        {
            foreach (var item in dBLiveRecordModel)
            {
                this.AddItem(item);
            }
        }
    }
}
