using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using static GenerateExample.MainWindow;
using ListViewItem = System.Windows.Controls.ListViewItem;

namespace GenerateExample
{
    public partial class MainWindow : Window
    {
        int countExamples = 0, countEndeavor = 0, countAnswer = 0;
        double countTryAnswer = 0.0;
        bool isCheck = false;

        public List<Example> examples { get; set; }

        public List<char> sumbol = new List<char>();
        public ObservableCollection<Example> Collection { get; set; }

        Random ran = new Random();

        public MainWindow()
        {
            InitializeComponent();
            Collection = new ObservableCollection<Example>();
            Base.IsChecked = true;
        }

        private void dgMain_SelectedCellsChanged_1(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count == 0) return;
            var currentCell = e.AddedCells[0];
            if (currentCell.Column ==
                exampleDataGrid.Columns[1])
            {
                exampleDataGrid.BeginEdit();
            }
        }

        private void GenerateAnswer(int countExample) 
        {
            //
            this.examples = new List<Example>();
            //
            int first, second;
            if (Base.IsChecked == true)
            {
                for (int i = 0; i < countExample; i++)
                {
                    Example example = new Example();

                    switch (sumbol[ran.Next(sumbol.Count)])
                    {
                        case '+':
                            {
                                first = ran.Next(10, 900);
                                second = ran.Next(10, 900);
                                example.example = first.ToString() + "+" + second.ToString();
                                break;
                            }
                        case '/':
                            {
                                first = ran.Next(100, 900);
                                second = ran.Next(2, 10);
                                while (first % second != 0)
                                {
                                    first++;
                                }
                                example.example = first.ToString() + "/" + second.ToString();
                                break;
                            }
                        case '-':
                            {
                                first = ran.Next(500, 900);
                                second = ran.Next(10, 499);
                                example.example = first.ToString() + "-" + second.ToString();
                                break;
                            }
                        case '*':
                            {
                                first = ran.Next(11, 300);
                                second = ran.Next(2, 10);
                                example.example = first.ToString() + "*" + second.ToString();
                                break;
                            }
                    }
                    example.id = i + 1;
                    //
                    this.examples.Add(example);
                    //
                }
            }
            else if(Table.IsChecked == true)
            {
                    for (int i = 0; i < countExample; i++)
                    {
                        Example example = new Example();

                        switch (sumbol[ran.Next(sumbol.Count)])
                        {
                            case '*':
                                {
                                    first = ran.Next(2, 10);
                                    second = ran.Next(2, 10);
                                    example.example = first.ToString() + " * " + second.ToString();
                                    break;
                                }
                            case '/':
                                {
                                    first = ran.Next(2, 10);
                                    second = ran.Next(2, 10);
                                    example.example = (first*second).ToString() + " / " + second.ToString();
                                    break;
                                }
                        }
                        example.id = i + 1;
                        //
                        this.examples.Add(example);
                        //
                    }
                }
            //
            foreach (var p in examples)
            {
                Collection.Add(p);
            }   
            exampleDataGrid.ItemsSource = Collection;
            //
        }

        private void GenerateBut_Click(object sender, RoutedEventArgs e)
        {
            if (countExampleBox.Text == "")
            { 
            MessageBox.Show("Число не введено", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
            }
            if (sumbol.Count == 0)
            {
                MessageBox.Show("Не выбран вид примеров", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (Convert.ToInt32(countExampleBox.Text) < 10)
            {
                MessageBox.Show("Оценка может высчитываться некорректно, если количество примеров меньше 10", "Предупреждение",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            countAnswer = 0;
            countEndeavor++;
            numberText.Text = countEndeavor.ToString();
            evaluationText.Text = "";
            Collection.Clear();
            countExamples = Convert.ToInt32(countExampleBox.Text);
            GenerateAnswer(Convert.ToInt32(countExampleBox.Text));
            isCheck = false;
            checkBut.IsEnabled = true;
            countTryAnswer = 0;
            evaluationText.Text = ""; 
        }

        private void exampleDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (isCheck)
            {
                Example example = (Example)e.Row.DataContext;
                if (example.answer != example.tryAnswer)
                {
                    e.Row.Background = new SolidColorBrush(Colors.Red);
                }
                else
                {
                    e.Row.Background = new SolidColorBrush(Colors.Green);
                }  
            }
        }

        private void CheckBut_Click(object sender, RoutedEventArgs e)
        {
            var selectedRow = new Example();
            for(int i = 0; i < countExamples; i++)
            {
                exampleDataGrid.SelectedIndex = i;
                selectedRow = (Example)exampleDataGrid.SelectedItem;
                selectedRow.tryAnswer = Convert.ToInt32(Calculator.Calc(selectedRow.example));
                if (Collection[i].tryAnswer == Collection[i].answer)
                {
                    countTryAnswer++;
                }
            }   
            double evaluationGrade = countTryAnswer / countExamples, evaluation = 0.0;
            if (evaluationGrade >= 0.90) evaluation = 5;
            else if (evaluationGrade >= 0.70) evaluation = 4;
            else if (evaluationGrade >= 0.50) evaluation = 3;
            else if (evaluationGrade < 0.50) evaluation = 2;
            evaluationText.Text = "Выполнено " + countTryAnswer + " из " + countExamples + ". Оценка: " + evaluation;
            checkBut.IsEnabled = false;
            isCheck = true;
            exampleDataGrid.ItemsSource = null;
            exampleDataGrid.ItemsSource = Collection;
        }

        private void minusCheck_Checked(object sender, RoutedEventArgs e)
        {
            sumbol.Add('-');
        }

        private void minusCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            sumbol.Remove('-');
        }

        private void plusCheck_Checked(object sender, RoutedEventArgs e)
        {
            sumbol.Add('+');
        }

        private void plusCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            sumbol.Remove('+');
        }

        private void divisionCheck_Checked(object sender, RoutedEventArgs e)
        {
            sumbol.Add('/');
        }

        private void divisionCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            sumbol.Remove('/');
        }

        private void multiCheck_Checked(object sender, RoutedEventArgs e)
        {
            sumbol.Add('*');
        }

        private void table_Checked(object sender, RoutedEventArgs e)
        {
            plusCheck.IsEnabled = false;
            plusCheck.IsChecked = false;
            minusCheck.IsEnabled = false;
            minusCheck.IsChecked = false;
        }

        private void base_Checked(object sender, RoutedEventArgs e)
        {
            plusCheck.IsEnabled = true;
            plusCheck.IsChecked = true;
            minusCheck.IsEnabled = true;
            minusCheck.IsChecked = true;
        }

        private void multiCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            sumbol.Remove('*');
        }

        void isSettings(bool isSet)
        {
            
        }

    }
}
