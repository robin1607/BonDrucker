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
    /// Interaktionslogik für Calculator.xaml
    /// </summary>
    public partial class Calculator : Window
    {
        decimal _price;
        decimal _givenMoney;
        decimal _lastValue;

        public Calculator(decimal price)
        {
            InitializeComponent();
            _price = price;
        }

        private void txtMoneyIn_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtMoneyIn.Text != null && _givenMoney > 0)
                {
                    decimal returnMoney = calc(_price, _givenMoney);
                    txtMoneyOut.Text = returnMoney + " €";
                }
                else
                {
                    txtMoneyOut.Text = "0,00 €";
                }
            }
            catch (Exception)
            {
                txtMoneyOut.Text = "0,00 €";
            }
        }

        private decimal calc(decimal price, decimal givenMoney)
        {
            return givenMoney - price;
        }

        private void button_currency_click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = sender as Button;
                string value = btn.Content.ToString().TrimEnd(' ', '€');
                decimal valueAsDecimal = Convert.ToDecimal(value);
                _givenMoney += valueAsDecimal;
                _lastValue = valueAsDecimal;
                updateTxtMoneyIn();
            }
            catch (Exception)
            {

            }

        }

        private void updateTxtMoneyIn()
        {
            try
            {
                txtMoneyIn.Text = (_givenMoney + " €");
            }
            catch (Exception)
            {

                txtMoneyIn.Text = "0,00 €";
            }
        }

        private void btn_09_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _givenMoney -= _lastValue;
                _lastValue = 0;
                updateTxtMoneyIn();
            }
            catch (Exception)
            {

            }

        }

        private void btn_10_Click(object sender, RoutedEventArgs e)
        {
            _givenMoney = 0;
            updateTxtMoneyIn();
        }

        private void btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
