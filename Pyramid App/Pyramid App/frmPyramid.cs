using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;
using gnuciDictionary;
using System.Drawing.Printing;

namespace Pyramid_App
{
    public partial class frmPyramid : Form
    {
        List<Word> lstword = gnuciDictionary.EnglishDictionary.GetAllWords().ToList();
        List<TextBox> lsttxtbx;
        List<TextBox> lstRow1;
        List<TextBox> lstRow2;
        List<TextBox> lstRow3;
        List<TextBox> lstRow4;
        List<TextBox> lstRow5;
        List<List<TextBox>> lstRows;
        List<Label> lstblocks;
        public frmPyramid()
        {
            InitializeComponent();

            txtRow11.Text = GetRandomStartLetter();
            lsttxtbx = new() { txtRow21, txtRow22, txtRow31, txtRow32, txtRow33, txtRow41, txtRow42, txtRow43, txtRow44, txtRow51, txtRow52, txtRow53, txtRow54, txtRow55 };
            lstRow1 = new() { txtRow11 };
            lstRow2 = new() { txtRow21, txtRow22 };
            lstRow3 = new() { txtRow31, txtRow32, txtRow33 };
            lstRow4 = new() { txtRow41, txtRow42, txtRow43, txtRow44 };
            lstRow5 = new() { txtRow51, txtRow52, txtRow53, txtRow54, txtRow55 };
            lstRows = new() { lstRow1, lstRow2, lstRow3, lstRow4, lstRow5 };
            lstblocks = new() { lblBlock3, lblBlock2, lblBlock1 };
            lsttxtbx.ForEach(tb => tb.TextChanged += Tb_TextChanged);
            btnRules.Click += BtnRules_Click;
        }
        private string GetRandomStartLetter()
        {
            string s = GetUpperLetters();
            List<Word> lstwordfiltered = new();
            lstwordfiltered.AddRange(lstword.Where(word => word.Value.Length == 2 && s.Contains(word.Value.ToUpper().Substring(0, 1)) && s.Contains(word.Value.ToUpper().Substring(1, 1))).ToList());
            Random rnd = new();
            int i = rnd.Next(1, lstwordfiltered.Count());
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

        private void Tb_TextChanged(object? sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                var tb = sender;
                int index = lsttxtbx.FindIndex(i => i == tb);
                int nextindex = index + 1;
                if (nextindex < lsttxtbx.Count() && lsttxtbx[index].Text != "")
                {
                    TextBox txtbx = lsttxtbx[nextindex];
                    txtbx.Enabled = true;
                    txtbx.Focus();
                }
                int lstindex = lstRows.FindIndex(row => row.Contains(tb));
                DetectCompleteRow(lstindex, nextindex);
            }

        }

        private void DetectCompleteRow(int index, int nextindex)
        {
            if (lstRows[index].Count(tb => tb.Text != "") == lstRows[index].Count)
            {
                DetectWord(index, nextindex);
            }
        }

        private void DetectWord(int index, int nextindex)
        {
            StringBuilder sb = new();
            lstRows[index].ForEach(tb => sb.Append(tb.Text));
            string s = sb.ToString();
            List<Word> lstcheckword = new();
            lstcheckword.AddRange(lstword.Where(word => word.Value.ToUpper().Equals(s)).ToList());
            bool b = lstcheckword.Count >= 1;
            bool bo = ContainsPreviousWord(index, s);
            if (b == true && bo == true)
            {
                lstRows[index].ForEach(tb => tb.BorderStyle = BorderStyle.FixedSingle);
                lstRows[index].ForEach(tb => tb.Enabled = false);
                Word word = lstcheckword.First();
                lblDefinition.Text = word.Definition;
                if (index == 4)
                {
                    GetScore();
                    GetNewPyramid();
                }
            }
            else
            {
                lstRows[index].ForEach(tb => tb.Clear());
                Label lbl = lstblocks.First(bl => bl.BackColor == Color.White);
                lbl.BackColor = Color.FromArgb(162, 251, 226);
                if (nextindex < lsttxtbx.Count())
                {
                    lsttxtbx[nextindex].Enabled = false;
                }
                lsttxtbx.First(tb => tb.Text == "").Focus();
                if (lstblocks.Count(bl => bl.BackColor == Color.White) == 0)
                {
                    GameOver();
                }
                else if (b == false)
                {
                    MessageBox.Show("The word you've entered isn't in the dictionary.");
                }
                else if (bo == false)
                {
                    MessageBox.Show("Must include previous letters");
                }
            }
        }

        private bool ContainsPreviousWord(int index, string s)
        {
            bool b = false;
            int previousindex = index - 1;
            int i = 0;
            foreach (TextBox tb in lstRows[previousindex])
            {
                bool bo = s.Contains(tb.Text);
                if (bo == true)
                {
                    i = i + 1;
                    if (i == lstRows[previousindex].Count())
                    {
                        b = true;
                    }
                }
            }
            return b;
        }

        private int ConvertToInt(string s)
        {
            int n = 0;
            bool b = int.TryParse(s, out n);
            return n;
        }

        private void GetScore()
        {
            int i = ConvertToInt(lblScoreVal.Text);
            i = i + 1;
            lblScoreVal.Text = i.ToString();
            CheckBestPlayed(i);
        }

        private void CheckBestPlayed(int score)
        {
            int scoreval = score;
            int bestplayed = ConvertToInt(lblBestPlayedVal.Text);
            if (scoreval > bestplayed)
            {
                lblBestPlayedVal.Text = scoreval.ToString();
            }
        }

        private void GetNewPyramid()
        {
            lsttxtbx.ForEach(tb => tb.Clear());
            txtRow11.Text = GetRandomStartLetter();
            lsttxtbx.ForEach(tb => tb.BorderStyle = BorderStyle.Fixed3D);
            lsttxtbx.ForEach(tb => tb.Enabled = false);
            txtRow21.Enabled = true;
            txtRow21.Focus();
        }

        private void GameOver()
        {
            MessageBox.Show("Game Over");
            GetNewPyramid();
            lstblocks.ForEach(bl => bl.BackColor = Color.White);
            lblScoreVal.Text = "0";
        }

        private void BtnRules_Click(object? sender, EventArgs e)
        {
            string s = "1. Write a word" + Environment.NewLine + "2. Include all words from the previous layer" + Environment.NewLine + "3. You get 3 tries to complete the pyramid";
            MessageBox.Show(s);
            lsttxtbx.First(tb => tb.Text == "").Focus();
        }
    }
}
