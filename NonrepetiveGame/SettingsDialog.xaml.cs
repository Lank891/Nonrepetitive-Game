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
using System.Windows.Shapes;

namespace NonrepetiveGame
{
    /// <summary>
    /// Logika interakcji dla klasy SettingsDialog.xaml
    /// </summary>
    public partial class SettingsDialog : Window
    {

        public string LengthRequiredToWin { get; set; }
        public int LengthRequiredToWinAsInt => Int32.Parse(LengthRequiredToWin);
        
        public string AiMovesAhead { get; set; }
        public int AiMovesAheadAsInt => Int32.Parse(AiMovesAhead);
        
        public string AllowedCharacters { get; set; }

        public SettingsDialog(int lengthRequiredToWin, int aiMovesAhead, IList<char> allowedCharacters)
        {
            LengthRequiredToWin = lengthRequiredToWin.ToString();
            AiMovesAhead = aiMovesAhead.ToString();
            AllowedCharacters = allowedCharacters.Aggregate("", (current, c) => current + c);
            DataContext = this;
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (LengthRequiredToWinAsInt < 3)
                {
                    MessageBox.Show("Length required to win must be 3 or more.");
                    return;
                }

                if (AiMovesAheadAsInt < 1)
                {
                    MessageBox.Show("Ai level must be 1 or more.");
                    return;
                }

                AllowedCharacters = String.Concat(AllowedCharacters.Where(c => !Char.IsWhiteSpace(c)));
                AllowedCharacters = AllowedCharacters.Distinct().OrderBy(c => c).Aggregate("", (current, c) => current + c);
                if (AllowedCharacters.Length < 3)
                {
                    MessageBox.Show("At least 3 distinct characters must be present.");
                    return;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input.");
                return;
            }


            this.DialogResult = true;
        }
    }
}
