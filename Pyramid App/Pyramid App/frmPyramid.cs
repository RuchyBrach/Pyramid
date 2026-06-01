using PyramidSystem;
namespace Pyramid_App
{
    public partial class frmPyramid : Form
    {
        Game game = new();
        List<TextBox> lsttxtbx;
        List<List<TextBox>> lstRows;
        List<Label> lstblocks;
        public frmPyramid()
        {
            InitializeComponent();
            lsttxtbx = new() { txtRow11, txtRow21, txtRow22, txtRow31, txtRow32, txtRow33, txtRow41, txtRow42, txtRow43, txtRow44, txtRow51, txtRow52, txtRow53, txtRow54, txtRow55 };

            lstRows = new()
            {
            new() { txtRow11 },
            new() { txtRow21, txtRow22 },
            new() { txtRow31, txtRow32, txtRow33 },
            new() { txtRow41, txtRow42, txtRow43, txtRow44 },
            new() { txtRow51, txtRow52, txtRow53, txtRow54, txtRow55 },
            };

            lstblocks = new() { lblBlock3, lblBlock2, lblBlock1 };

            for (int row = 0; row < lstRows.Count; row++)
            {
                for (int col = 0; col < lstRows[row].Count; col++)
                {
                    TextBox tb = lstRows[row][col];

                    tb.DataBindings.Add("Text", game.Rows[row].LetterTiles[col], "LetterTileValue");
                    tb.TextChanged += Tb_TextChanged;
                }
            }

            lblScoreVal.DataBindings.Add("Text", game, "Score");
            //lblBestPlayedVal.DataBindings.Add("Text", game, "BestPlayed");
            lblDefinition.DataBindings.Add("Text", game, "CurrentDefinition");
            for (int i = 0; i < lstblocks.Count; i++)
            {
                lstblocks[i].DataBindings.Add(
                    "BackColor",
                    game.Blocks[i],
                    "BackColor");
            }
            this.Load += FrmPyramid_Load;
            btnRules.Click += BtnRules_Click;

        }

        private void FrmPyramid_Load(object? sender, EventArgs e)
        {
            StartGame();
        }


        private void StartGame()
        {
            game.StartGame();
            lstRows[game.CurrentRowIndex].First().Focus();
        }

        private void Tb_TextChanged(object? sender, EventArgs e)
        {

            if (sender is TextBox tb)
            {
                tb.DataBindings["Text"].WriteValue();

                int row = lstRows.FindIndex(r => r.Contains(tb));

                if (game.UpdateRowProgress(row) == false)
                {
                    lstRows[row].ForEach(txt => txt.Enabled = true);
                    if (game.Message != "")
                    {
                        MessageBox.Show(game.Message);
                    }
                    if (game.RemainingAttempts == 0)
                    {
                        StartGame();
                    }

                    else
                    {
                        lstRows[row].First().Focus();
                    }
                    return;
                }

                int index = lsttxtbx.FindIndex(i => i == tb);
                int nextindex = index + 1;
                if (nextindex < lsttxtbx.Count && lsttxtbx[index].Text != "")
                {
                    TextBox txtbx = lsttxtbx[nextindex];
                    txtbx.Enabled = true;
                    txtbx.Focus();

                    int nextRow = lstRows.FindIndex(r => r.Contains(txtbx));
                    int nextCol = lstRows[nextRow].IndexOf(txtbx);

                    if (nextCol == 0 && nextRow > 0)
                    {
                        lstRows[nextRow - 1].ForEach(tb => tb.Enabled = false);
                    }
                }
            }
        }

        private void BtnRules_Click(object? sender, EventArgs e)
        {
            MessageBox.Show(game.Rules);
            lsttxtbx.First(tb => tb.Text == "").Focus();
        }
    }
}
