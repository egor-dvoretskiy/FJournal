using FJournalGUI.ViewModels;
using FJournalLib;
using FJournalLib.Repositories;
using FJournalLib.Enums;
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
using FJournalGUI.Models.Filter;

namespace FJournalGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationViewModel _applicationViewModel;

        public MainWindow()
        {
            InitializeComponent();

            this._applicationViewModel = new ApplicationViewModel();
            this.DataContext = this._applicationViewModel;

            this.grid_TitleBar.MouseLeftButtonDown += grid_TitleBar_MouseLeftButtonDown;

            this.UpdateFilterSettingsGroupboxValues();
        }

        private void grid_TitleBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.DragMove();

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

        private void button_ApplyFilterSettings_Click(object sender, RoutedEventArgs e)
        {
            FilterSettingsViewModel filterSettingsViewModel = new FilterSettingsViewModel();

            bool isValidCalculatedAmountOfRecordsToDisplay = int.TryParse(textbox_AmountOfRecordsToDisplay.Text, out int amountOfRecordsToDisplay);
            if (isValidCalculatedAmountOfRecordsToDisplay)
                filterSettingsViewModel.AmountOfRecordsToDisplay = amountOfRecordsToDisplay;

            this._applicationViewModel.UpdateFilterSettingsViewModel(filterSettingsViewModel);
            this.UpdateFilterSettingsGroupboxValues();

            this.dg_dbRecords.ItemsSource = this._applicationViewModel.Records;
        }

        private void UpdateFilterSettingsGroupboxValues()
        {
            var values = this._applicationViewModel.FilterSettingsViewModel;

            this.textbox_AmountOfRecordsToDisplay.Text = values.AmountOfRecordsToDisplay.ToString();
        }

        private void button_ResetFilterSettingsToDefault_Click(object sender, RoutedEventArgs e)
        {
            this.textbox_AmountOfRecordsToDisplay.Text = DefaultFilterSettings.AmountOfRecordsToDisplay.ToString();
        }
    }
}
