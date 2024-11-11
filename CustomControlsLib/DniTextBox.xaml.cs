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
    public partial class DniTextBox : UserControl
    {

        public static readonly DependencyProperty DniProperty = DependencyProperty.Register(
            "DniText",
            typeof(string),
            typeof(DniTextBox),
            new FrameworkPropertyMetadata(
                string.Empty,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnDniChanged));

        public static readonly DependencyProperty TooltipMessageDniProperty =
            DependencyProperty.Register("TooltipMessageDni", typeof(string), typeof(DniTextBox), new PropertyMetadata(string.Empty));

        public string DniText
        {
            get => (string)GetValue(DniProperty);
            set => SetValue(DniProperty, value);
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

        public string TooltipMessageDni
        {
            get { return (string)GetValue(TooltipMessageDniProperty); }
            set { SetValue(TooltipMessageDniProperty, value); }
        }

        public DniTextBox()
        {
            InitializeComponent();
            dniTextBox.TextChanged += (s, e) => DniText = dniTextBox.Text;
        }

     

        private static void OnDniChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (DniTextBox)d;
            var newValue = (string)e.NewValue;
            if (control != null)
            {
                if (control.dniTextBox.Text != newValue)
                {
                    control.dniTextBox.Text = newValue;
                }
                control.Validate(newValue);
            }
        }

        private void Validate(string dni)
        {
            string dniPattern = @"^\d{8}[A-Za-z]$";

            IsValid = Regex.IsMatch(dniTextBox.Text, dniPattern);


            if (!IsValid && !string.IsNullOrEmpty(dniTextBox.Text))
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                TooltipMessageDni = "Text must be 8 numbers and 1 letter";
            }
            else
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
                TooltipMessageDni = string.Empty;
            }
        }
    }
}
