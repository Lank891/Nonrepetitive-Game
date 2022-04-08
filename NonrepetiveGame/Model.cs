using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonrepetiveGame
{
    public class Model : INotifyPropertyChanged
    {
        private readonly ObservableCollection<char> _characters = new(new char[] { 'A', 'B', 'C' });
        /// <summary>
        /// All possible characters
        /// </summary>
        public ObservableCollection<char> Characters => _characters;

        
        private int _finalLength = 10;
        /// <summary>
        /// Lenght of the word that human player have to reach
        /// </summary>
        public int FinalLength
        {
            get => _finalLength;
            set
            {
                _finalLength = value;
                OnPropertyChanged(nameof(FinalLength));
            }
        }

        private int _aiMovesAhead = 1;
        /// <summary>
        /// Number of turns AI will consider before making a move
        /// </summary>
        public int AiMovesAhead
        {
            get => _aiMovesAhead;
            set
            {
                _aiMovesAhead = value;
                OnPropertyChanged(nameof(AiMovesAhead));
            }
        }

        private string _word = "";
        /// <summary>
        /// Actually present word
        /// </summary>
        public string Word
        {
            get => _word;
            set
            {
                _word = value;
                OnPropertyChanged(nameof(Word));
                OnPropertyChanged(nameof(RemainingCharacters));
            }
        }

        /// <summary>
        /// Number of characters left to be added
        /// </summary>
        public int RemainingCharacters => FinalLength - Word.Length;

        private bool _isGameOver = false;
        /// <summary>
        /// Is the game finished (either computer or player won)?
        /// </summary>
        public bool IsGameOver
        {
            get => _isGameOver;
            set
            {
                _isGameOver = value;
                OnPropertyChanged(nameof(IsGameOver));
                OnPropertyChanged(nameof(ControlButtonsEnabled));
                OnPropertyChanged(nameof(CharacterButtonsEnabled));
            }
        }

        private bool _isAiTurn = false;
        /// <summary>
        /// Is it the AI turn?
        /// </summary>
        public bool IsAiTurn
        {
            get => _isAiTurn;
            set
            {
                _isAiTurn = value;
                OnPropertyChanged(nameof(IsAiTurn));
                OnPropertyChanged(nameof(ControlButtonsEnabled));
                OnPropertyChanged(nameof(CharacterButtonsEnabled));
            }
        }

        /// <summary>
        /// Should control buttons (settings, reset) be enabled?
        /// </summary>
        public bool ControlButtonsEnabled => !IsAiTurn;

        /// <summary>
        /// Should buttons with characters be enabled?
        /// </summary>
        public bool CharacterButtonsEnabled => !IsGameOver && !IsAiTurn;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
