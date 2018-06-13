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
        
        public MainWindow()
        {
            InitializeComponent();
            this.ReadFile("C:/test/tasks.txt");
            TableList.Source = ListOfTasks;
            this.table.ItemsSource = TableList.View;
            TableList.Filter += new FilterEventHandler(All_Checked);
        }
        private void ReadFile(string path)
        {
            try
            {   // Open the text file using a stream reader.
                using (StreamReader file = new StreamReader(path))
                {
                    string line;
                    while ((line = file.ReadLine()) != null)
                    {
                        string[] words = line.Split('|');
                        DateTime date = DateTime.ParseExact(words[0], "yyyy-MM-dd",
                                       System.Globalization.CultureInfo.InvariantCulture);
                        ListOfTasks.Add(new Task(date, words[1], Int32.Parse(words[2]),words[3]));        
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
        private void All_Checked(object sender, FilterEventArgs e)
        {
            Task task = e.Item as Task;
            if (task != null)
            {
                // Filter out products with price 25 or above
                if (notcompleted.IsChecked == true && task.Completion < 100)
                {
                    e.Accepted = true;
                }
                else
                {
                    e.Accepted = false;
                }
            }
        }

        private void Overdue_Checked(object sender)
        {

        }

        private void Today_Checked(object sender, FilterEventArgs e)
        {

        }

        private void Thisweek_Checked(object sender, FilterEventArgs e)
        {

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Add win = new Add();
            win.ShowDialog();
            win.added = false;
            if (win.added == true)
            {
                ListOfTasks.Add(new Task(win.datepick.SelectedDate.Value,win.title.Text,(int)win.slider.Value,win.descrpition.Text));
                TableList.View.Refresh();
            }
        }
        private void Notcompleted_Selected(object sender, RoutedEventArgs e)
        {

        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
