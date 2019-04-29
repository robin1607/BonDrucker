using BonDrucker.Models;
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

namespace BonDrucker
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<MealCombination> _mealCombos = new List<MealCombination>();
        public decimal _totalPrice {get; set;}
        public MainWindow()
        {
            InitializeComponent();
            addButtonsToForm();
            dataGrid.ItemsSource = _mealCombos;
            bntPrint.IsEnabled = false;
            txtBoxTotalPrice.DataContext = this;
        }

        private void addButtonsToForm()
        {
            List<IMeal> mainMeals = getMainMealsFromCSV();
            btnGrid.Children.Clear();
            int index = 0;
            int indexRow = 0;
            btnGrid.ColumnDefinitions.Clear();
            btnGrid.RowDefinitions.Clear();
            btnGrid.RowDefinitions.Add(new RowDefinition());
            foreach (IMeal meal in mainMeals)
            {
                Button newBtn = new Button();
                newBtn.Content = meal.mealName;
                newBtn.Tag = meal;
                newBtn.FontSize = 18;
                newBtn.IsEnabled = !meal.soldOut;
                newBtn.MinHeight = 120;
                newBtn.MinWidth = 100;
                newBtn.Margin = new Thickness(5,0,5,0);
                newBtn.Name = meal.insertable.ToString()  + "_mainMealButton";
                newBtn.Click += new RoutedEventHandler(mainMealButton_click);
                if (index < 6 && indexRow == 0)
                {
                    btnGrid.ColumnDefinitions.Add(new ColumnDefinition());
                }
                if (index == 6)
                {
                    btnGrid.RowDefinitions.Add(new RowDefinition());
                    indexRow++;
                    index = 0;
                }
                Grid.SetRow(newBtn, indexRow);
                Grid.SetColumn(newBtn, index);

                index++;
                btnGrid.Children.Add(newBtn);
            }
        }

        private List<IMeal> getMainMealsFromCSV()
        {
            return CSVHandler.readMeals("BonDrucker.MainMeal");
        }

        private void closeProgramm(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void editMeals(object sender, RoutedEventArgs e)
        {
            mainMealEditer mE = new mainMealEditer();
            mE.ShowDialog();
            addButtonsToForm();
        }

        protected void mainMealButton_click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            var mainMeal = button.Tag as MainMeal;
            if (mainMeal.insertable)
            {
                secondMealChooser smc = new secondMealChooser(mainMeal);
                smc.Owner = this;
                if (smc.ShowDialog() == false)
                {
                    MealCombination combo = smc._mealCombo;
                    if (combo != null)
                    {
                        updateTotalPriceTxtBox(combo.totalPrice);
                        addMealCombosToDataGrid(combo);
                    }

                }
            }
            else
            {
                addSingleMainMealToDataGrid(mainMeal);
            }
            bntPrint.IsEnabled = true;

        }

        private void addMealCombosToDataGrid(MealCombination combo)
        {
            _mealCombos.Add(combo);
            dataGrid.Items.Refresh();
        }

        private void addSingleMainMealToDataGrid(MainMeal mainMeal)
        {
            SecondMeal secondMeal = new SecondMeal();
            secondMeal.price = 0;
            secondMeal.mealName = "ohne";
            MealCombination mc = MealCombination.getMealCombination(mainMeal, secondMeal);
            updateTotalPriceTxtBox(mainMeal.price);
            addMealCombosToDataGrid(mc);
        }


        // Keine Optimale Loesung .. aber funktioniert vorerst
        private void updateTotalPriceTxtBox(decimal price)
        {
            _totalPrice += price;
            txtBoxTotalPrice.Text = _totalPrice + " €";
        }

        private void resetTotalPriceTxtBox()
        {
            _totalPrice = 0;
            txtBoxTotalPrice.Text = _totalPrice + " €";
        }

        private void resetForm()
        {
            _mealCombos = new List<MealCombination>();
            resetTotalPriceTxtBox();
            bntPrint.IsEnabled = false;
            dataGrid.ItemsSource = _mealCombos;
        }

        private void bntDelete_Click(object sender, RoutedEventArgs e)
        {
            resetForm();
        }

        private void bntPrint_Click(object sender, RoutedEventArgs e)
        {
            if(_mealCombos.Count > 0)
            {
                CSVHandler.addToStatistic(_mealCombos);
                PrintingHandler.Print(_mealCombos);
                Calculator c = new Calculator(_totalPrice);
                c.Owner = this;
                c.ShowDialog();
                resetForm();
            }
        }

        private void showStatistic(object sender, RoutedEventArgs e)
        {
            StatisticViewer sv = new StatisticViewer();
            sv.ShowDialog();
        }
    }
}
