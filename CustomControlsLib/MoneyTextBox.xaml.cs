using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CustomControlsLib
{
    /// <summary>
    /// Lógica de interacción para MoneyTextBox.xaml
    /// </summary>
    public partial class MoneyTextBox : UserControl
    {
        public static readonly DependencyProperty MoneyProperty = DependencyProperty.Register(
           "MoneyText",
           typeof(string),
           typeof(MoneyTextBox), new FrameworkPropertyMetadata(
               string.Empty,
               FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
               OnMoneyChanged));

        public static readonly DependencyProperty TooltipMessageMoneyProperty =
            DependencyProperty.Register("TooltipMessageMoney", typeof(string), typeof(MoneyTextBox), new PropertyMetadata(string.Empty));

        public string MoneyText
        {
            get => (string)GetValue(MoneyProperty);
            set => SetValue(MoneyProperty, value);
        }

        public MoneyTextBox()
        {
            InitializeComponent();
            moneyTextBox.TextChanged += (s, e) => MoneyText = moneyTextBox.Text;
            moneyTextBox.PreviewTextInput += OnPreviewTextInput;
        }

        public string Email
        {
            get => moneyTextBox.Text;
            set => moneyTextBox.Text = value;
        }

        public string TooltipMessageMoney
        {
            get { return (string)GetValue(TooltipMessageMoneyProperty); }
            set { SetValue(TooltipMessageMoneyProperty, value); }
        }

        private void OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, @"^\d*$");
        }

        private static void OnMoneyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MoneyTextBox)d;
            var newValue = (string)e.NewValue;
            if (control != null)
            {
                if (control.moneyTextBox.Text != newValue)
                {
                    control.moneyTextBox.Text = newValue;
                }
                control.Validate(newValue);
            }
        }

        private void Validate(string pin)
        {
            string moneyPattern = @"^\d*$";

            if (!Regex.IsMatch(moneyTextBox.Text, moneyPattern) && !string.IsNullOrEmpty(moneyTextBox.Text))
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                TooltipMessageMoney = $"If no saving or debt, leave untouched";
            }
            else
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
                TooltipMessageMoney = string.Empty;
            }

        }

    }
}
