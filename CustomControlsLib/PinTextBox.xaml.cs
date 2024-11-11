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
    /// Lógica de interacción para PinTextBox.xaml
    /// </summary>
    public partial class PinTextBox : UserControl
    {
        public static readonly DependencyProperty pinProperty = DependencyProperty.Register(
            "PinText",
            typeof(string),
            typeof(PinTextBox), new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnPinChanged));
        public static readonly DependencyProperty TooltipMessagePinProperty =
            DependencyProperty.Register("TooltipMessagePin", typeof(string), typeof(PinTextBox), new PropertyMetadata(string.Empty));


        public string PinText
        {
            get => (string)GetValue(pinProperty);
            set => SetValue(pinProperty, value);
        }

        public string TooltipMessagePin
        {
            get { return (string)GetValue(TooltipMessagePinProperty); }
            set { SetValue(TooltipMessagePinProperty, value); }
        }

        public static readonly DependencyProperty IsValidProperty = DependencyProperty.Register(
            "IsValid",
            typeof(bool),
            typeof(PhoneMaskTextBox),
            new PropertyMetadata(false));  // Default to false

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            private set => SetValue(IsValidProperty, value);
        }

        public PinTextBox()
        {
            InitializeComponent();
            pinTextBox.TextChanged += (s, e) => PinText = pinTextBox.Text;
        }

        public string Email
        {
            get => pinTextBox.Text;
            set => pinTextBox.Text = value;
        }


        private static void OnPinChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PinTextBox)d;
            var newValue = (string)e.NewValue;
            if (control != null)
            {
                if (control.pinTextBox.Text != newValue)
                {
                    control.pinTextBox.Text = newValue;
                }
                control.Validate(newValue);
            }
        }

        private void Validate(string pin)
        {
            string pinPattern = @"^\d{4}$";
            IsValid = Regex.IsMatch(pinTextBox.Text, pinPattern);

            if (!IsValid && !string.IsNullOrEmpty(pinTextBox.Text))
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                TooltipMessagePin = $"Pin must be 4 characters.";
            }
            else
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
                TooltipMessagePin = string.Empty;
            }

        }

    }
}
