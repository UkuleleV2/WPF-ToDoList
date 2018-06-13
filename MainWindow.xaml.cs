using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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

namespace ToDoApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Task> ListOfTasks = new List<Task>();
        CollectionViewSource TableList = new CollectionViewSource();
        string currentfile;
        public MainWindow()
        {
            InitializeComponent();
            currentfile = "tasks.tskk";
            this.ReadFile(currentfile);
            TableList.Source = ListOfTasks;
            this.table.ItemsSource = TableList.View;
            all.IsChecked = true;
        }
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Add win = new Add();
            win.ShowDialog();
            if (win.added == true)
            {
                ListOfTasks.Add(new Task(win.datepick.SelectedDate.Value, win.title.Text, (int)win.slider.Value, win.descrpition.Text));
                TableList.View.Refresh();
            }
        }
        private void Row_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            DataRowView dr = table.SelectedItem as DataRowView;
            var selected = table.SelectedItem as Task;
            Add win = new Add();
            
            win.added = false;
            win.title.Text = selected.Title;
            win.datepick.SelectedDate = selected.Date;
            win.descrpition.Text = selected.Description;
            win.slider.Value = selected.Completion;
            win.ShowDialog();
            win.added = true;
            if (win.added == true)
            {
                selected.Date = win.datepick.SelectedDate.Value;
                selected.Title = win.title.Text;
                selected.Completion = (int)win.slider.Value;
                selected.Description = win.descrpition.Text;
                TableList.View.Refresh();
            }

        }
        private void AllFilter(object sender, RoutedEventArgs e)
        {
            TableList.Filter += new FilterEventHandler(All_Checked);
        }
        private void OverdueFilter(object sender, RoutedEventArgs e)
        {
            TableList.Filter += new FilterEventHandler(Overdue_Checked);
        }
        private void ThisweekFilter(object sender, RoutedEventArgs e)
        {
            TableList.Filter += new FilterEventHandler(Thisweek_Checked);
        }
        private void TodayFilter(object sender, RoutedEventArgs e)
        {
            TableList.Filter += new FilterEventHandler(Today_Checked);
        }
        private void All_Checked(object sender, FilterEventArgs e)
        {
            Task task = e.Item as Task;
            if (task != null)
            {
                // Filter out products with price 25 or above
                if (notcompleted.IsChecked == true)
                    if(task.Completion<100)
                        {
                            e.Accepted = true;
                        }
                        else
                        {
                            e.Accepted = false;
                        }
                else
                {
                    e.Accepted = true;
                }
            }
        }


        private void Overdue_Checked(object sender, FilterEventArgs e)
        {
            Task task = e.Item as Task;
            if (task != null)
            {
                // Filter out products with price 25 or above
                if (notcompleted.IsChecked == true)
                    if (task.Completion < 100 && task.Date < DateTime.Today)
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                else 
                {
                    if (task.Date < DateTime.Today)
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }
        }

        private void Today_Checked(object sender, FilterEventArgs e)
        {
            Task task = e.Item as Task;
            DateTime current = DateTime.Now;
            if (task != null)
            {
                if (notcompleted.IsChecked == true)
                    if (task.Completion < 100 && task.Date == DateTime.Today)
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                else
                {
                    if (task.Date == DateTime.Today)
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }
        }

        private void Thisweek_Checked(object sender, FilterEventArgs e)
        {
            Task task = e.Item as Task;
            DateTime current = DateTime.Today.AddDays(7);
            if (task != null)
            {
                // Filter out products with price 25 or above
                if (notcompleted.IsChecked == true)
                    if (task.Completion < 100 && task.Date < DateTime.Today.AddDays(7) && task.Date >= DateTime.Today)
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                else
                {
                    if (task.Date < DateTime.Today.AddDays(7) && task.Date >= DateTime.Today)
                    {
                        e.Accepted = true;
                    }
                    else
                    {
                        e.Accepted = false;
                    }
                }
            }
        }

        private void Notcompleted_Selected(object sender, RoutedEventArgs e)
        {
            if(all.IsChecked == true)
                TableList.Filter += new FilterEventHandler(All_Checked);
            else if (overdue.IsChecked == true)
                TableList.Filter += new FilterEventHandler(Overdue_Checked);
            else if (thisweek.IsChecked == true)
                TableList.Filter += new FilterEventHandler(Thisweek_Checked);
            else if(today.IsChecked == true)
                TableList.Filter += new FilterEventHandler(Today_Checked);

        }
        private void ReadFile(string path)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader file = new StreamReader(path))
                {
                    ListOfTasks.Clear();
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] words = line.Split('|');
                        DateTime date = DateTime.ParseExact(words[0], "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
                        ListOfTasks.Add(new Task(date, words[1], Int32.Parse(words[2]), words[3]));
                    }
                    file.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (StreamWriter outputFile = new StreamWriter(currentfile, true))
            {
                for (int i = 0; i < ListOfTasks.Count;i++)
                {
                        outputFile.WriteLine(ListOfTasks[i].Date.ToString("yyyy-MM-dd") +"|"+ ListOfTasks[i].Title + "|" + ListOfTasks[i].Completion.ToString()+"|"+ ListOfTasks[i].Description);
                }
                MessageBox.Show("File Saved!");

            }
        }
        private void New_File(object sender, RoutedEventArgs e)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".tskk"; // Default file extension
            dlg.Filter = "Task files (.tskk)|*.tskk"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                currentfile = dlg.FileName;
            }
        }

        private void Open_File(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Document"; // Default file name
            dlg.DefaultExt = ".tskk"; // Default file extension
            dlg.Filter = "Task files (.tskk)|*.tskk"; // Filter files by extension

            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                currentfile = dlg.FileName;
                this.ReadFile(currentfile);
                TableList.View.Refresh();
            }
        }
    }
}
