using System.Text;
using gnuciDictionary;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;

namespace PyramidSystem
{
    public class Game : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler? BestPlayedChanged;

        public List<Word> lstword = gnuciDictionary.EnglishDictionary.GetAllWords().ToList();
        int _currentrowindex = 1;
        string _currentdefinition = "";
        int _score = 0;
        string _message = "";
        private static int numgames;

        public Game()
        {
            numgames++;
            this.GameName = "Game " + numgames;
            for (int i = 0; i < 5; i++)
            {
                Row row = new();
                for (int l = 0; l <= i; l++)
                {
                    row.LetterTiles.Add(new LetterTile());
                }
                this.Rows.Add(row);
            }
            for (int i = 0; i < 3; i++)
            {
                this.Blocks.Add(new Block());
            }
        }

        public List<Row> Rows { get; private set; } = new();
        public List<Block> Blocks { get; private set; } = new();
        public string GameName { get; private set; }
        public string ScoreLabelText { get => "Score: (" + this.GameName + ")";}
        public string BlocksLabelText { get => "Blocks: (" + this.GameName + ")"; }
        public string GameDescription { get => $"Current Game: " + this.GameName; }
        public int CurrentRowIndex
        {
            get => _currentrowindex;
            set
            {
                _currentrowindex = value;
                InvokePropertyChanged();
            }
        }

        public string CurrentDefinition
        {
            get => _currentdefinition;
            set
            {
                _currentdefinition = value;
                InvokePropertyChanged();
            }
        }
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                this.InvokePropertyChanged();

            }
        }
        public static int BestPlayed { get; private set; } = 0;
        public string Rules { get => $"1. Write a word {Environment.NewLine}2. Include all letters from the previous layer {Environment.NewLine}3. You get 3 tries to complete the pyramid"; }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                this.InvokePropertyChanged();
            }
        }
        public int RemainingAttempts { get; private set; } = 3;

        public void StartGame()
        {
            Score = 0;
            CurrentDefinition = "";
            Blocks.ForEach(b => b.BackColor = Color.White);
            RemainingAttempts = 3;
            GetNewPyramid();
        }
        private string GetRandomStartLetter()
        {
            string s = GetUpperLetters();
            List<Word> lstwordfiltered = new();
            lstwordfiltered.AddRange(lstword.Where(word => word.Value.Length == 2 && s.Contains(word.Value.ToUpper().Substring(0, 1)) && s.Contains(word.Value.ToUpper().Substring(1, 1))).ToList());
            Random rnd = new();
            int i = rnd.Next(0, lstwordfiltered.Count());
            string startletter = lstwordfiltered[i].Value.Substring(rnd.Next(0, 2), 1).ToUpper();
            return startletter;
        }
        private string GetUpperLetters()
        {
            StringBuilder sb = new();
            for (int i = 1; i < 27; i++)
            {
                int allletters = i + 64;
                sb.Append((char)allletters);
            }
            return sb.ToString();
        }
        public void SetScore()
        {
            Score++;
            CheckBestPlayed(Score);
        }
        public void CheckBestPlayed(int score)
        {
            if (score > BestPlayed)
            {
                BestPlayed = score;
            }
            BestPlayedChanged?.Invoke(this, new EventArgs());
        }
        public bool UpdateRowProgress(int rowindex)
        {
            bool b = true;
            Row row = Rows[rowindex];
            if (rowindex == 0)
            {
                return b;
            }
            if (row.DetectCompleteRow())
            {
                b = DetectWord(row);
            }
            return b;
        }
        public bool DetectWord(Row row)
        {
            if (row.IsCorrect)
            {
                return true;
            }
            StringBuilder sb = new();
            row.LetterTiles.ForEach(l => sb.Append(l.LetterTileValue));
            string s = sb.ToString().ToUpper();
            List<Word> lstcheckword = new();
            lstcheckword.AddRange(lstword.Where(word => word.Value.ToUpper().Equals(s)).ToList());
            bool b = lstcheckword.Count >= 1;
            bool bo = ContainsPreviousWord(row, s);
            if (b == true && bo == true)
            {
                row.IsCorrect = true;
                Word word = lstcheckword.FirstOrDefault(w => !string.IsNullOrWhiteSpace(w.Definition)) ?? lstcheckword.First();
                CurrentDefinition = word.Definition;
                if (row == Rows.Last())
                {
                    SetScore();
                    GetNewPyramid();
                }
                else
                {
                    CurrentRowIndex = CurrentRowIndex + 1;
                }
            }
            else
            {
                row.clear();
                RemainingAttempts = RemainingAttempts - 1;
                Block? bl = Blocks.FirstOrDefault(bl => bl.BackColor == System.Drawing.Color.White);
                if (bl != null) { bl.BackColor = System.Drawing.Color.FromArgb(162, 251, 226); }
                if (Blocks.Count(bl => bl.BackColor == System.Drawing.Color.White) == 0)
                {
                    GameOver();
                }
                else if (b == false)
                {
                    Message = "The word you've entered isn't in the dictionary.";
                }
                else if (bo == false)
                {
                    Message = "Must include previous letters";
                }
            }
            return b && bo;
        }
        public bool ContainsPreviousWord(Row row, string word)
        {
            int index = Rows.IndexOf(row);
            if (index == 0)
            {
                return true;
            }
            int previousindex = index - 1;
            List<char> letters = word.ToUpper().ToList();
            foreach(LetterTile tile in Rows[previousindex].LetterTiles)
            {
                char letter = tile.LetterTileValue.ToUpper().First();
                if(letters.Contains(letter) == false)
                {
                    return false;
                }
                letters.Remove(letter);
            }
            
            return true;
        }
        public void GetNewPyramid()
        {
            CurrentRowIndex = 1;
            Rows.ForEach(r => { r.clear(); r.IsCorrect = false; });
            Rows[0].LetterTiles[0].LetterTileValue = GetRandomStartLetter();
        }
        public void GameOver()
        {
            Message = "Game Over";
        }
        private void InvokePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
