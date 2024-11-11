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

namespace CustomControlsLib
{
    /// <summary>
    /// Interaction logic for MinLengthTextBox.xaml
    /// </summary>
    public partial class MinLengthTextBox : UserControl
    {

        public static readonly DependencyProperty MinLengthProperty =
        DependencyProperty.Register("MinLength", typeof(int), typeof(MinLengthTextBox), new PropertyMetadata(0));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(MinLengthTextBox),
                new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, onTextBoxChanged));

        public static readonly DependencyProperty TooltipMessageProperty =
            DependencyProperty.Register("TooltipMessage", typeof(string), typeof(MinLengthTextBox), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IsValidProperty =
            DependencyProperty.Register("IsValid", typeof(bool), typeof(MinLengthTextBox), new PropertyMetadata(false));

        public bool IsValid
        {
            get { return (bool)GetValue(IsValidProperty); }
            set { SetValue(IsValidProperty, value); }
        }

        public int MinLength
        {
            get { return (int)GetValue(MinLengthProperty); }
            set { SetValue(MinLengthProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string TooltipMessage
        {
            get { return (string)GetValue(TooltipMessageProperty); }
            set { SetValue(TooltipMessageProperty, value); }
        }

        public MinLengthTextBox()
        {
            InitializeComponent();
            nameTextBox.TextChanged += (s, e) => Text = nameTextBox.Text;
        }

        private static void onTextBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MinLengthTextBox)d;
            var newValue = (string)e.NewValue;

            if (control.nameTextBox.Text != newValue)
            {
                control.nameTextBox.Text = newValue;
            }

            if (!string.IsNullOrEmpty(newValue))
            {
                control.Validate(newValue);
            }
        }

        private void Validate(string name)
        {
            bool isValid = name.Length >= MinLength;
            IsValid = isValid; // Update IsValid property

            if (!isValid)
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Red);
                TooltipMessage = $"Text length must be at least {MinLength} characters.";
            }
            else
            {
                OuterBorder.BorderBrush = new SolidColorBrush(Colors.Gray);
                TooltipMessage = string.Empty;
            }
        }
    }
}
