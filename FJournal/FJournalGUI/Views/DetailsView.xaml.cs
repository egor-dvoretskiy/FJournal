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
using System.Windows.Shapes;

namespace FJournalGUI.Views
{
    /// <summary>
    /// Interaction logic for DetailsView.xaml
    /// </summary>
    public partial class DetailsView : Window
    {
        private readonly DetailsViewModel _detailsViewModel;

        public DetailsView(DBRecordModel? model, string title)
        {
            InitializeComponent();

            this._detailsViewModel = new DetailsViewModel(model, title);
            this.grid_TitleBar.MouseLeftButtonDown += grid_TitleBar_MouseLeftButtonDown;

            this.DataContext = this._detailsViewModel;
        }

        private void grid_TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        private void button_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
