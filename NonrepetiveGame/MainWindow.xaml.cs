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

namespace NonrepetiveGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Model _model;

        private const string _playerWon = "Player won!";
        private const string _computerWon = "Computer won!";
        private const string _playing = "";

        public MainWindow()
        {
            _model = new Model();
            DataContext = _model;
            ChangeSettings();
            InitializeComponent();
        }
        
        /// <summary>
        /// Opens dialog with settings (actual ones as default value) and allows to change them 
        /// </summary>
        /// <returns>True if 'Confirm' was clicked (even with no changes), False otherwise</returns>
        private bool ChangeSettings()
        {
            var settings = new SettingsDialog(_model.FinalLength, _model.AiMovesAhead, _model.Characters);
            if (settings.ShowDialog().GetValueOrDefault() == true)
            {
                _model.FinalLength = settings.LengthRequiredToWinAsInt;
                _model.AiMovesAhead = settings.AiMovesAheadAsInt;
                _model.Characters.Clear();
                foreach (var character in settings.AllowedCharacters)
                {
                    _model.Characters.Add(character);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Button click for changing settings, resets the game if settings were 'Confirmed' (even with no changes)
        /// </summary>
        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            bool changesConfirmed = ChangeSettings();

            if(changesConfirmed)
                Reset_Click(sender, e);
        }

        /// <summary>
        /// Resets the game
        /// </summary>
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            _model.Word = "";
            WinInfoBox.Text = _playing;
            _model.IsAiTurn = false;
            _model.IsGameOver = false;
        }

        /// <summary>
        /// Adds character from the tag of clicked character button, and if game is not finished, processes AI turn
        /// </summary>
        private void CharacterButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessPlayerTurn((char)((Button)sender).Tag);
            if(!_model.IsGameOver)
            {
                ProcessAITurn();
            } 
        }

        /// <summary>
        /// Checks if the game is finished and sets proper values
        /// </summary>
        private void CheckForFinish()
        {
            if(_model.RemainingCharacters <= 0)
            {
                _model.IsGameOver = true;
                WinInfoBox.Text = _playerWon;
            }

            if(CheckForRepetition(out int repetitionLength))
            {
                _model.IsGameOver = true;
                WinInfoBox.Text = _computerWon;

                // TODO: Use those parts to show the repetition nicely
                string nonRepeatedPart = _model.Word[..^(2 * repetitionLength)];
                string repeatedPart = _model.Word[^repetitionLength..];
                // Word = nonRepeatedPart + repeatedPart + repeatedPart
                
                // Won't work for long repetition, ideally it should be shown somehow in text box where the word is
                WinInfoBox.Text += " Repeated part: " + repeatedPart;
            }
        }

        /// <summary>
        /// Checks if repetition occured
        /// </summary>
        /// <returns>If repetition occured</returns>
        private bool CheckForRepetition(out int repetitionLength)
        {
            repetitionLength = 0;

            // We check repetitions of length (of one half) from 2 to n/2
            for (int i = 2; i <= _model.Word.Length/2; i++ )
            {
                repetitionLength = i;
                // We care only about repetitions contaiing the last letter, since repetitions in previous part of the word were checked already
                string left = _model.Word.Substring(_model.Word.Length - 2 * i, i);
                string right = _model.Word.Substring(_model.Word.Length - i, i);
                if (left == right)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Processes the player turn by adding new character and checking if game is finished
        /// </summary>
        /// <param name="character"></param>
        private void ProcessPlayerTurn(char character)
        {
            _model.Word += character;
            CheckForFinish();
        }

        /// <summary>
        /// Processes the AI turn by adding its character and checking if game is finished
        /// 
        /// <para>TODO: Implement AI strategy</para>
        /// </summary>
        private async void ProcessAITurn()
        {
            //TODO: Add AI logic
            _model.IsAiTurn = true;

            //This one is kinda important, without it (or if value is low enough) the window won't redraw until AI stops their move,
            //  so we won't be able to show in any way that player cannot input anything. This delay must basically be enough to disable
            //  buttons (and other things) that we want to disable
            await Task.Delay(50); 

            //Simulate delay
            long sum = 0;
            for (int i = 0; i < 10e7; i++)
            {
                sum += i;
            }

            _model.Word += _model.Characters.GetRandomElement();

            CheckForFinish();
            _model.IsAiTurn = false;
        }
    }
}
