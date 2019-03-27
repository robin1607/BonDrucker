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
        public MainWindow()
        {
            InitializeComponent();
            addButtonsToForm();
        }

        private void addButtonsToForm()
        {
            List<IMeal> mainMeals = getMainMealsFromCSV();
            spButton.Children.Clear();
            foreach(IMeal meal in mainMeals)
            {
                Button newBtn = new Button();

                newBtn.Content = meal.mealName;
                newBtn.Name = meal.insertable.ToString()  + "_mainMealButton";
                newBtn.Click += new RoutedEventHandler(mainMealButton_click);

                spButton.Children.Add(newBtn);
            }

        }

        private List<IMeal> getMainMealsFromCSV()
        {
            return CSVHandler.read("BonDrucker.MainMeal");

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
            if (getInsertableOfButton(button))
            {
                secondMealChooser smc = new secondMealChooser(getInsertableOfButton(button));
                smc.ShowDialog();
            }

        }

        private bool getInsertableOfButton(Button button)
        {
            char insertable = button.Name[0];
            return insertable == 'T' ? true : false;
        }
    }
}
