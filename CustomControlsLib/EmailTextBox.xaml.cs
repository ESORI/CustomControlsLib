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
    public partial class EmailTextBox : UserControl
    {
        public static readonly DependencyProperty EmailProperty = DependencyProperty.Register(
            "EmailText",
            typeof(string),
            typeof(EmailTextBox), new FrameworkPropertyMetadata(
                string.Empty,       
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnEmailChanged));

        public static readonly DependencyProperty TooltipMessageEmailProperty =
            DependencyProperty.Register("TooltipMessageEmail", typeof(string), typeof(EmailTextBox), new PropertyMetadata(string.Empty));


        public string EmailText
        {
            get => (string)GetValue(EmailProperty);
            set => SetValue(EmailProperty, value);
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

        public EmailTextBox()
        {
            InitializeComponent();
            emailTextBox.TextChanged += (s, e) => EmailText = emailTextBox.Text;
        }

        public string Email
        {
            get => emailTextBox.Text;
            set => emailTextBox.Text = value;
        }

        public string TooltipMessageEmail
        {
            get { return (string)GetValue(TooltipMessageEmailProperty); }
            set { SetValue(TooltipMessageEmailProperty, value); }
        }

        private static void OnEmailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (EmailTextBox)d;
            var newValue = (string)e.NewValue;
            if (control != null)
            {
                if (control.emailTextBox.Text != newValue)
                {
                    control.emailTextBox.Text = newValue;
                }
                control.Validate(newValue);
            }
        }

        private void Validate(string email)
        {
            string emailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            IsValid = Regex.IsMatch(emailTextBox.Text, emailPattern);

            if (!IsValid && !string.IsNullOrEmpty(emailTextBox.Text))
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                TooltipMessageEmail = $"Email must be similar to: example@mail.com";
            }
            else
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
                TooltipMessageEmail = string.Empty;
            }

        }

    }
}
