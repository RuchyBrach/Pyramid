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
        List<TextBox> lstRow2;
        List<TextBox> lstRow3;
        List<TextBox> lstRow4;
        List<TextBox> lstRow5;
        List<List<TextBox>> lstRows;
        List<Label> lstblocks;
        public frmPyramid()
        {
            InitializeComponent();
            lblStartLetter.Text = GetRandomStartLetter();
            lsttxtbx = new() { txtRow21, txtRow22, txtRow31, txtRow32, txtRow33, txtRow41, txtRow42, txtRow43, txtRow44, txtRow51, txtRow52, txtRow53, txtRow54, txtRow55 };
            lstRow2 = new() { txtRow21, txtRow22 };
            lstRow3 = new() { txtRow31, txtRow32, txtRow33 };
            lstRow4 = new() { txtRow41, txtRow42, txtRow43, txtRow44 };
            lstRow5 = new() { txtRow51, txtRow52, txtRow53, txtRow54, txtRow55 };
            lstRows = new() { lstRow2, lstRow3, lstRow4, lstRow5 };
            lstblocks = new() { lblBlock3, lblBlock2, lblBlock1 };
            lsttxtbx.ForEach(tb => tb.TextChanged += Tb_TextChanged);
        }

        private string GetRandomStartLetter()
        {
            List<Word> lstwordfiltered = new();
            lstwordfiltered.AddRange(lstword.Where(word => word.Value.Length == 2).ToList());
            Random rnd = new();
            int i = rnd.Next(1, lstwordfiltered.Count());
            string startletter = lstwordfiltered[i].Value.Substring(rnd.Next(0, 2), 1).ToUpper();
            return startletter;
        }

        private void Tb_TextChanged(object? sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                var tb = sender;
                int index = lsttxtbx.FindIndex(i => i == tb);
                int nextindex = index + 1;
                if (nextindex < lsttxtbx.Count())
                {
                    TextBox txtbx = lsttxtbx[nextindex];
                    txtbx.Enabled = true;
                    txtbx.Focus();
                }
                int lstindex = lstRows.FindIndex(row => row.Contains(tb));
                DetectCompleteRow(lstindex);
            }

        }

        private void DetectCompleteRow(int index)
        {
            if (lstRows[index].Count(tb => tb.Text != "") == lstRows[index].Count)
            {
                DetectWord(index);
            }
        }

        private void DetectWord(int index)
        {
            string previousword = GetPreviousWord(index);
            StringBuilder sb = new();
            lstRows[index].ForEach(tb => sb.Append(tb.Text));
            string s = sb.ToString();
            List<Word> lstcheckword = new();
            lstcheckword.AddRange(lstword.Where(word => word.Value.ToUpper().Equals(s)).ToList());
            bool b = lstcheckword.Count >= 1;
            bool bo = s.Contains(previousword);
            if (b == true && bo == true)
            {
                lstRows[index].ForEach(tb => tb.BorderStyle = BorderStyle.FixedSingle);
                if (index == 3)
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
                if (b == false) 
                {
                    MessageBox.Show("The word you've entered isn't in the dictionary."); 
                }
                else if (bo == false)
                {
                    MessageBox.Show("Must include previous letters");
                }
            } 
        }

        private string GetPreviousWord(int index)
        {
            int previousindex = index - 1;
            StringBuilder sb = new();
            lstRows[previousindex].ForEach(tb => sb.Append(tb.Text));
            string s = sb.ToString();
            return s;
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
        }

        private void GetNewPyramid()
        {
            lblStartLetter.Text = "";
            lsttxtbx.ForEach(tb => tb.Clear());
            lblStartLetter.Text = GetRandomStartLetter();
            lsttxtbx.ForEach(tb => tb.BorderStyle = BorderStyle.Fixed3D);
        }
    }
}
