using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PyramidSystem
{
    public class LetterTile : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        string _lettertilevalue = "";
        public string LetterTileValue
        {
            get => _lettertilevalue;
            set
            {
                _lettertilevalue = value;
                this.InvokePropertyChanged();
            }
        }

        private void InvokePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }
    }
}
