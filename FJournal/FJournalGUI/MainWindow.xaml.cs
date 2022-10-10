using FJournalGUI.ViewModels;
using FJournalGUI.Views;
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
using FJournalGUI.Models;

namespace FJournalGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ApplicationViewModel _applicationViewModel;

        /*private int timerIncrement = 0;*/

        public MainWindow()
        {
            InitializeComponent();

            // inner initialization
            this._applicationViewModel = new ApplicationViewModel();
            this.DataContext = this._applicationViewModel;
            // -

            // events
            this.grid_TitleBar.MouseDown += Grid_TitleBar_MouseDown;
            this.grid_TitleBar.OnClose += Grid_TitleBar_OnClose;
            this.grid_TitleBar.OnMaximize += Grid_TitleBar_OnMaximize;
            this.grid_TitleBar.OnMinimize += Grid_TitleBar_OnMinimize;
            // -

            // update layout
            this.textblock_AmountOfItemsInRecords.Content = this._applicationViewModel.Records is null ? "0" : this._applicationViewModel.Records.Count().ToString();
            this.UpdateFilterSettingsGroupboxValues();
            // -

            /*Timer timer = new Timer();
            timer.Interval = 1000;
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;*/
        }

        private void Grid_TitleBar_OnMinimize() => this.WindowState = WindowState.Minimized;

        private void Grid_TitleBar_OnMaximize() => this.WindowState = WindowState.Maximized;

        private void Grid_TitleBar_OnClose() => this.Close();

        private void Grid_TitleBar_MouseDown(object sender, MouseButtonEventArgs e) => this.DragMove();

        /*private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Journal journal = new Journal();
            journal.Note(new FJournalLib.Models.TRecord()
            {
                Message = $"{++this.timerIncrement}",
                LogType = LogType.Info
            });
        }*/

        private void dg_dbRecords_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dg_dbRecords.SelectedItem == null)
                return;

            var selectedRow = dg_dbRecords.SelectedItem as DBRecordModel;

            DetailsView detailsViewWindow = new DetailsView(selectedRow, this.Title);
            detailsViewWindow.Show();

            dg_dbRecords.UnselectAll();
        }

        private void button_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_ApplyFilterSettings_Click(object sender, RoutedEventArgs e)
        {
            FilterSettingModel filterSettingsViewModel = new FilterSettingModel();

            // get amount of items from ui
            bool isValidCalculatedAmountOfRecordsToDisplay = int.TryParse(textbox_AmountOfRecordsToDisplay.Text, out int amountOfRecordsToDisplay);
            if (isValidCalculatedAmountOfRecordsToDisplay)
                filterSettingsViewModel.AmountOfRecordsToDisplay = amountOfRecordsToDisplay;
            // -

            // get selected dates from calendar
            var selectedDates = this.calendar_DateFilter.SelectedDates;
            if (selectedDates != null)
            {
                filterSettingsViewModel.DateTimes = selectedDates;
            }
            // -

            // get message span to search in messages
            filterSettingsViewModel.MessageSpan = this.textbox_MessageSpan.Text;
            // -

            // update filter in data context
            this._applicationViewModel.UpdateFilterSettingsViewModel(filterSettingsViewModel);
            this.UpdateFilterSettingsGroupboxValues();
            // -

            // updating layout
            this.dg_dbRecords.ItemsSource = this._applicationViewModel.Records;
            this.textblock_AmountOfItemsInRecords.Content = this._applicationViewModel.Records.Count().ToString();
            this.textblock_elapsed.Content = this._applicationViewModel.Elapsed.ToString();
            // -
        }

        private void UpdateFilterSettingsGroupboxValues()
        {
            var values = this._applicationViewModel.FilterSettingsModel;

            this.textbox_AmountOfRecordsToDisplay.Text = values.AmountOfRecordsToDisplay.ToString();
        }

        private void button_ResetFilterSettingsToDefault_Click(object sender, RoutedEventArgs e)
        {
            this.textbox_AmountOfRecordsToDisplay.Text = DefaultFilterSettings.AmountOfRecordsToDisplay.ToString();
            this.calendar_DateFilter.SelectedDates.Clear();
        }

        private void button_ChooseDate_Click(object sender, RoutedEventArgs e)
        {
            switch (this.grid_Calendar.Visibility)
            {
                case Visibility.Visible:
                    this.grid_Calendar.Visibility = Visibility.Collapsed;
                    break;
                case Visibility.Collapsed:
                default:
                    this.grid_Calendar.Visibility = Visibility.Visible;
                    break;
            }
        }

        private int GetIndexForDatagridByHeader(string? columnName, DataGrid dataGrid)
        {
            if (!string.IsNullOrEmpty(columnName))
            {
                foreach (var item in dataGrid.Columns)
                {
                    if (item is null)
                        continue;

                    if ((item.Header as string) == columnName)
                    {
                        return item.DisplayIndex;
                    }
                }
            }           

            return -1;
        }

        private void ProcessMenuItemVisibility(MenuItem? menuItem)
        {
            if (menuItem is null)
                return;

            int columnIndex = this.GetIndexForDatagridByHeader(menuItem.Header as string, this.dg_dbRecords);

            if (columnIndex == -1)
                return;

            this.dg_dbRecords.Columns[columnIndex].Visibility = menuItem.IsChecked ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TimeStampMenuItem_Click(object sender, RoutedEventArgs e) => this.ProcessMenuItemVisibility(sender as MenuItem);

        private void LogSourceMenuItem_Click(object sender, RoutedEventArgs e) => this.ProcessMenuItemVisibility(sender as MenuItem);

        private void LogTypeMenuItem_Click(object sender, RoutedEventArgs e) => this.ProcessMenuItemVisibility(sender as MenuItem);

        private void MessageMenuItem_Click(object sender, RoutedEventArgs e) => this.ProcessMenuItemVisibility(sender as MenuItem);

        private void CallerMemberNameMenuItem_Click(object sender, RoutedEventArgs e) => this.ProcessMenuItemVisibility(sender as MenuItem);

        private void CallerFilePathMenuItem_Click(object sender, RoutedEventArgs e) => this.ProcessMenuItemVisibility(sender as MenuItem);

        private void CallerLineNumberMenuItem_Click(object sender, RoutedEventArgs e) => this.ProcessMenuItemVisibility(sender as MenuItem);

        private void TotalCpuUsageMenuItem_Click(object sender, RoutedEventArgs e) => this.ProcessMenuItemVisibility(sender as MenuItem);

        private void PrivateMemoryUsageMenuItem_Click(object sender, RoutedEventArgs e) => this.ProcessMenuItemVisibility(sender as MenuItem);

        private void TabControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var control = sender as TabControl;

            if (control is null)
            {
                return;
            }

            foreach(TabItem item in control.Items)
            {
                double newWidth = (control.ActualHeight / control.Items.Count) - 2;
                if (newWidth < 0)
                    newWidth = 0;

                item.Width = newWidth;
            }
        }

        private void TabControl_Main_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var tabControl = sender as TabControl;
            var tabItem = tabControl?.SelectedItem as TabItem;

            if (tabItem is null || !this.IsLoaded)
                return;

            if (tabItem.Header.Equals("Live Window"))
            {
                this.LiveBlock_Records.EnableLiveBlock();

                this.button_ApplyFilterSettings.IsEnabled = false;
                this.button_ResetFilterSettingsToDefault.IsEnabled = false;
            }
            else
            {
                this.LiveBlock_Records.DisableLiveBlock();

                this.button_ApplyFilterSettings.IsEnabled = true;
                this.button_ResetFilterSettingsToDefault.IsEnabled = true;
            }
        }

    }
}
