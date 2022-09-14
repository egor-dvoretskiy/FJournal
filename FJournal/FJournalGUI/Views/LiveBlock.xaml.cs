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
    /// Interaction logic for LiveBlock.xaml
    /// </summary>
    public partial class LiveBlock : UserControl
    {
        private int itemsCountLimit = 50;
        public LiveBlock()
        {
            InitializeComponent();
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

        public void AddItem(DBLiveRecordModel liveBlockItemModel)
        {
            this.DeleteExtraItemsIfNecessary();
            this.StackPanel_Records.Children.Add(new LiveBlockItem(liveBlockItemModel));
        }

        private void DeleteExtraItemsIfNecessary()
        {
            while (this.StackPanel_Records.Children.Count >= itemsCountLimit)
            {
                this.StackPanel_Records.Children.RemoveAt(0);
            }
        }
    }
}
