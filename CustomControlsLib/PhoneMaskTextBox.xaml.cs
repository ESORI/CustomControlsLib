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
    /// Interaction logic for PhoneMaskTextBox.xaml
    /// </summary>
    public partial class PhoneMaskTextBox : UserControl
    {

        // Dependency Property for PhoneNumber
        public static readonly DependencyProperty PhoneNumberProperty = DependencyProperty.Register(
            "PhoneNumber",
            typeof(string),
            typeof(PhoneMaskTextBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnPhoneNumberChanged));

        public string PhoneNumber
        {
            get => (string)GetValue(PhoneNumberProperty);
            set => SetValue(PhoneNumberProperty, value);
        }

        private bool _isValid;

        public bool IsValid
        {
            get => _isValid;
            private set
            {
                _isValid = value;

            }
        }

        // Dependency Property for Mask
        public static readonly DependencyProperty MaskProperty =
            DependencyProperty.Register("Mask", typeof(string), typeof(PhoneMaskTextBox), new PropertyMetadata("+34 ### ### ###"));

        public string Mask
        {
            get => (string)GetValue(MaskProperty);
            set => SetValue(MaskProperty, value);
        }

        public static readonly DependencyProperty TooltipMessageProperty = DependencyProperty.Register(
        "TooltipMessage",
        typeof(string),
        typeof(PhoneMaskTextBox),
        new PropertyMetadata(string.Empty));

        public string TooltipMessage
        {
            get => (string)GetValue(TooltipMessageProperty);
            set => SetValue(TooltipMessageProperty, value);
        }
        public PhoneMaskTextBox()
        {
            InitializeComponent();
            this.phoneTextBox.PreviewTextInput += PhoneTextBox_PreviewTextInput;
            this.phoneTextBox.TextChanged += PhoneTextBox_TextChanged;
            this.phoneTextBox.PreviewKeyDown += PhoneTextBox_PreviewKeyDown;  // Add event for key press
            this.Loaded += PhoneMaskTextBox_Loaded;  // Handle the Loaded event to set caret
        }

        private void PhoneMaskTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            OuterBorder.BorderBrush = Brushes.Gray;

        }

        private bool hasStartedTyping = false;  // Flag to detect if typing has started

        private void PhoneTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // If this is the first time the user is typing
            if (!hasStartedTyping)
            {
                // Check if the typed character is a number (this could be customized if needed)
                if (Regex.IsMatch(e.Text, "^[0-9]+$"))
                {
                    hasStartedTyping = true;  // Mark that typing has started

                    // Move the caret to the first editable position (the first '#')
                    SetCaretToFirstEditablePosition();
                }
            }

            // Allow only numeric input
            if (!Regex.IsMatch(e.Text, "^[0-9]+$"))
            {
                e.Handled = true;  // Prevent non-numeric input
            }
        }

        private void SetCaretToFirstEditablePosition()
        {
            // Iterate through the mask and find the first '#' position
            for (int i = 0; i < Mask.Length; i++)
            {
                if (Mask[i] == '#')
                {
                    // Move the caret to the first '#' placeholder
                    phoneTextBox.CaretIndex = i;
                    break;
                }
            }
        }

        private void PhoneTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back)
            {
                int caretIndex = phoneTextBox.CaretIndex;
                string text = phoneTextBox.Text;

                if (caretIndex <= Mask.IndexOf('#'))
                {
                    e.Handled = true;  // Stop further processing if trying to delete before the editable part
                    return;
                }
                // If caret is at a space, move it back to the previous position
                if (caretIndex > 0 && text[caretIndex - 1] == ' ')
                {
                    phoneTextBox.CaretIndex--;  // Move caret past the space
                }

            }
        }

        private void PhoneTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string rawText = phoneTextBox.Text?.Replace("+34", "").Replace(" ", "") ?? "";

            // Limit to a maximum of 9 digits (excluding country code and spaces)
            if (rawText.Length > 9)
            {
                rawText = rawText.Substring(0, 9);
            }

            // Format the text according to the mask
            string formattedText = FormatText(rawText);

            // Only update text if it has changed
            if (phoneTextBox.Text != formattedText)
            {
                int currentCaretIndex = phoneTextBox.CaretIndex;
                phoneTextBox.Text = formattedText;

                // Place caret at next available # placeholder or the end
                int newCaretIndex = GetCaretIndexForTyping(formattedText, currentCaretIndex);
                phoneTextBox.CaretIndex = newCaretIndex;
            }

            // Update PhoneNumber property and validate the phone number
            PhoneNumber = rawText;

            // Validate the phone number after updating
            ValidatePhoneNumber();

        }

        private int GetCaretIndexForTyping(string formattedText, int currentCaretIndex)
        {
            // Move the caret to the next available # placeholder in the mask
            for (int i = currentCaretIndex; i < formattedText.Length; i++)
            {
                if (formattedText[i] == '#')
                {
                    return i;
                }
            }

            // If no # is found, place it at the end of the formatted text
            return formattedText.Length;
        }

        private string FormatText(string input)
        {
            // If input is empty, return the mask with all # symbols
            if (string.IsNullOrEmpty(input))
            {
                return Mask;
            }

            // Prepare a StringBuilder to build the formatted phone number
            var sb = new StringBuilder();
            int index = 0;

            // Iterate through the mask and replace '#' with the corresponding character from input
            foreach (char c in Mask)
            {
                if (c == '#' && index < input.Length)
                {
                    sb.Append(input[index]);  // Add digit in place of #
                    index++;
                }
                else if (c != '#')
                {
                    sb.Append(c);  // Append fixed characters (e.g., +, spaces)
                }
                else
                {
                    sb.Append('#');  // If no digit, keep the # in place
                }
            }

            return sb.ToString();
        }

        private void ValidatePhoneNumber()
        {
            // Extract raw digits by removing the country code and spaces
            string rawText = phoneTextBox.Text?.Replace("+34", "").Replace(" ", "") ?? "";

            // Count the number of numeric digits
            int filledDigits = rawText.Count(c => char.IsDigit(c));

            // Set validity based on the number of digits
            IsValid = filledDigits == 9; // We need exactly 9 digits

            // Change the border color based on validity
            OuterBorder.BorderBrush = IsValid ? Brushes.Gray : Brushes.Red;
            TooltipMessage = IsValid ? "Phone number is valid." : "Phone number must be 9 digits.";


        }

        private static void OnPhoneNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (PhoneMaskTextBox)d;
            var newValue = (string)e.NewValue;

            if (control != null && control.phoneTextBox != null)
            {
                // Update the TextBox with the formatted phone number
                if (control.phoneTextBox.Text != newValue)
                {
                    control.phoneTextBox.Text = control.FormatText(newValue);
                }
                if (!string.IsNullOrEmpty(newValue))
                {
                    control.ValidatePhoneNumber();
                }
            }
        }
    }
}

