using FJournalGUI.ViewModels;
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
        public MainWindow()
        {
            InitializeComponent();

            this.grid_TitleBar.MouseLeftButtonDown += Grid_TitleBar_MouseLeftButtonDown;

            this.DataContext = new ApplicationViewModel();
        }

        private void Grid_TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void dg_dbRecords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dg_dbRecords.SelectedItem == null)
                return;

            var selectedRow = dg_dbRecords.SelectedItem as DBRecordViewModel;

            if (selectedRow != null)
                MessageBox.Show($"You've picked the wrong house, fool. Message: {selectedRow.Message}");

            dg_dbRecords.UnselectAll();
        }

        private void button_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
