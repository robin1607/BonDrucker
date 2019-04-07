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
            txtBoxTotalPrice.DataContext = this;
        }

        private void addButtonsToForm()
        {
            List<IMeal> mainMeals = getMainMealsFromCSV();
            spButton.Children.Clear();
            foreach(IMeal meal in mainMeals)
            {
                Button newBtn = new Button();

                newBtn.Content = meal.mealName;
                newBtn.Tag = meal;
                newBtn.Name = meal.insertable.ToString()  + "_mainMealButton";
                newBtn.Click += new RoutedEventHandler(mainMealButton_click);

                spButton.Children.Add(newBtn);
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
        }

        protected void mainMealButton_click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            var mainMeal = button.Tag as MainMeal;
            if (mainMeal.insertable)
            {
                secondMealChooser smc = new secondMealChooser(mainMeal);
                if(smc.ShowDialog() == false)
                {
                    MealCombination combo = smc._mealCombo;
                    _totalPrice += combo.totalPrice;
                    txtBoxTotalPrice.DataContext = this;
                    addMealCombosToDataGrid(combo);
                }
            }
        }

        private void addMealCombosToDataGrid(MealCombination combo)
        {
            _mealCombos.Add(combo);
            dataGrid.Items.Refresh();
        }
    }
}
