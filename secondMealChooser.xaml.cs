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
using System.Windows.Shapes;

namespace BonDrucker
{
    /// <summary>
    /// Interaktionslogik für secondMealChooser.xaml
    /// </summary>
    public partial class secondMealChooser : Window
    {
        MainMeal _mainMeal;
        public MealCombination _mealCombo { get; set; }

        public secondMealChooser()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        public secondMealChooser(MainMeal mainMeal)
        {
            InitializeComponent();
            _mainMeal = mainMeal;
            addButtonsToForm();
        }

        private void addButtonsToForm()
        {
            List<MealCombination> mealCombos = CSVHandler.getMealCombinationsFromCSV(_mainMeal);
            
            grid.Children.Clear();
            int index = 0;
            foreach (MealCombination combo in mealCombos)
            {
                Button newBtn = new Button();
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                newBtn.Content = combo.secondMealName;
                newBtn.Name = "secondMealButton";
                newBtn.Tag = combo;
                newBtn.IsEnabled = !CSVHandler.isSecondMealDeactivated(combo.secondMealGUID);
                newBtn.Margin = new Thickness(5, 0, 5, 0);
                newBtn.FontSize = 18;
                newBtn.MinHeight = 200;
                newBtn.Click += new RoutedEventHandler(secondMealButton_click);

                Grid.SetColumn(newBtn, index);
                index++;
                grid.Children.Add(newBtn);
            }
        }

        protected void secondMealButton_click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            _mealCombo = button.Tag as MealCombination;
            this.Close();
        }
    }
}
