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
    public partial class IbanTextBox : UserControl
    {
        public static readonly DependencyProperty IbanProperty = DependencyProperty.Register(
            "IbanText",
            typeof(string),
            typeof(IbanTextBox), new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnIbanChanged));

        public static readonly DependencyProperty TooltipMessageIbanProperty =
            DependencyProperty.Register("TooltipMessageIban", typeof(string), typeof(IbanTextBox), new PropertyMetadata(string.Empty));

        public string IbanText
        {
            get => (string)GetValue(IbanProperty);
            set => SetValue(IbanProperty, value);
        }

        public IbanTextBox()
        {
            InitializeComponent();
            ibanTextBox.TextChanged += (s, e) => IbanText = ibanTextBox.Text;
        }

        public string TooltipMessageIban
        {
            get { return (string)GetValue(TooltipMessageIbanProperty); }
            set { SetValue(TooltipMessageIbanProperty, value); }
        }

        private static void OnIbanChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (IbanTextBox)d;
            var newValue = (string)e.NewValue;
            if ((control != null))
            {
                if(control.ibanTextBox.Text != newValue)
                {
                    control.ibanTextBox.Text = newValue;
                }
                control.Validate(newValue);
            }
        }

        private void Validate(string iban)
        {
            string ibanPattern = @"^\d{24}$";

            if(!Regex.IsMatch(ibanTextBox.Text, ibanPattern) && !string.IsNullOrEmpty(ibanTextBox.Text))
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                TooltipMessageIban = $"Iban is a total of 24 numbers";
            }
            else
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
                TooltipMessageIban = string.Empty;
            }
        }
    }
}
