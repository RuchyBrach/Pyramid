using PyramidSystem;

namespace PyramidMAUI;

public partial class Pyramid : ContentPage
{
    Game game = new();
    List<Entry> lstentries;
    List<List<Entry>> lstRows;
    List<Label> lstblocks;
    public Pyramid()
    {
        InitializeComponent();
        BindingContext = game;
        lstentries = new() { txtRow11, txtRow21, txtRow22, txtRow31, txtRow32, txtRow33, txtRow41, txtRow42, txtRow43, txtRow44, txtRow51, txtRow52, txtRow53, txtRow54, txtRow55 };

        lstRows = new()
            {
            new() { txtRow11 },
            new() { txtRow21, txtRow22 },
            new() { txtRow31, txtRow32, txtRow33 },
            new() { txtRow41, txtRow42, txtRow43, txtRow44 },
            new() { txtRow51, txtRow52, txtRow53, txtRow54, txtRow55 },
            };
        lstblocks = new() { lblBlock3, lblBlock2, lblBlock1 };
        lstentries.ForEach(entry => entry.TextChanged += Entry_TextChanged);
        this.Loaded += Pyramid_Loaded;
    }

    

    private void Pyramid_Loaded(object sender, EventArgs e)
    {
        StartGame();
    }
    private void StartGame()
    {
        game.StartGame();
        lstRows[game.CurrentRowIndex].First().Focus();
    }
    private void Entry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is Entry entry)
        {
            //entry.DataBindings["Text"].WriteValue();

            int row = lstRows.FindIndex(r => r.Contains(entry));

            if (game.UpdateRowProgress(row) == false)
            {
                lstRows[row].ForEach(e => e.IsEnabled = true);
                if (game.Message != "")
                {
                    DisplayAlert("Pyramid", game.Message, "Ok");
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

            int index = lstentries.FindIndex(i => i == entry);
            int nextindex = index + 1;
            if (nextindex < lstentries.Count && lstentries[index].Text != "")
            {
                Entry nextentry = lstentries[nextindex];
                nextentry.IsEnabled = true;
                nextentry.Focus();

                int nextRow = lstRows.FindIndex(r => r.Contains(nextentry));
                int nextCol = lstRows[nextRow].IndexOf(nextentry);

                if (nextCol == 0 && nextRow > 0)
                {
                    lstRows[nextRow - 1].ForEach(tb => tb.IsEnabled = false);
                }
            }
        }
    }

    private async void btnRules_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("Rules", game.Rules, "Ok");
        lstentries.FirstOrDefault(tb => tb.Text == "")?.Focus();
    }
}