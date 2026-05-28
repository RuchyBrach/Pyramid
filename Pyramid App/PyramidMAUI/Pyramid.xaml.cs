
using PyramidSystem;

namespace PyramidMAUI;

public partial class Pyramid : ContentPage
{
    Game activegame;
    List<Game> lstgame = new() { new Game(), new Game(), new Game() };
    List<Entry> lstentries;
    List<List<Entry>> lstRows;

    public Pyramid()
    {
        InitializeComponent();
        lstgame.ForEach(g => g.BestPlayedChanged += G_BestPlayedChanged);
        RBGame1.BindingContext = lstgame[0];
        RBGame2.BindingContext = lstgame[1];
        RBGame3.BindingContext = lstgame[2];
        activegame = lstgame[0];
        this.BindingContext = activegame;
        lstentries = new() { txtRow11, txtRow21, txtRow22, txtRow31, txtRow32, txtRow33, txtRow41, txtRow42, txtRow43, txtRow44, txtRow51, txtRow52, txtRow53, txtRow54, txtRow55 };

        lstRows = new()
            {
            new() { txtRow11 },
            new() { txtRow21, txtRow22 },
            new() { txtRow31, txtRow32, txtRow33 },
            new() { txtRow41, txtRow42, txtRow43, txtRow44 },
            new() { txtRow51, txtRow52, txtRow53, txtRow54, txtRow55 },
            };
        lstentries.ForEach(entry => entry.TextChanged += Entry_TextChanged);
        this.Loaded += Pyramid_Loaded;
    }

    private void G_BestPlayedChanged(object sender, EventArgs e)
    {
        lblBestPlayedValue.Text = Game.BestPlayed.ToString();
    }

    private void Pyramid_Loaded(object sender, EventArgs e)
    {
        lstgame.ForEach(g => g.StartGame());
    }
    private void StartGame()
    {
        activegame.StartGame();
        lstRows[activegame.CurrentRowIndex].First().Focus();
    }

    private async void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            entry.Unfocus();
            int row = lstRows.FindIndex(r => r.Contains(entry));

            if (activegame.UpdateRowProgress(row) == false)
            {
                lstRows[row].ForEach(e => e.IsEnabled = true);
                if (activegame.Message != "")
                {
                    await DisplayAlert("Pyramid", activegame.Message, "OK");
                }
                if (activegame.RemainingAttempts == 0)
                {
                    StartGame();
                }

                else
                {
                    lstRows[row].First().IsEnabled = true;
                    lstRows[row].First().Focus();
                }
                return;
            }

            int index = lstentries.FindIndex(i => i == entry);
            int nextindex = index + 1;
            if (nextindex < lstentries.Count && lstentries[index].Text != "")
            {
                Entry nextentry = lstentries[nextindex];
                nextentry.IsEnabled = true;
                nextentry.Focus();
            }
            if (activegame.Rows[row].IsCorrect)
            {
                lstRows[row].ForEach(e => e.IsEnabled = false);
            }
        }
    }

    private async void btnRules_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Rules", activegame.Rules, "OK");
        lstentries.FirstOrDefault(entry => entry.Text == "")?.Focus();
    }

    private void Game_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        RadioButton rb = (RadioButton)sender;
        if (rb.IsChecked && rb.BindingContext != null)
        {
            activegame = (Game)rb.BindingContext;
            this.BindingContext = activegame;
        }
    }
}