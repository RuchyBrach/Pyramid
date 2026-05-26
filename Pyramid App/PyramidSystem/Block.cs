using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PyramidSystem
{
    public class Block : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        System.Drawing.Color _backcolor = System.Drawing.Color.White;
        public System.Drawing.Color BackColor
        {
            get => _backcolor;
            set
            {
                _backcolor = value;
                this.InvokePropertyChanged();
                this.InvokePropertyChanged("BackColorMaui");
            }
        }

        public Microsoft.Maui.Graphics.Color BackColorMaui
        {
            get => this.ConvertToMauiColor(this.BackColor);
        }
        private void InvokePropertyChanged([CallerMemberName] string propertyname = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyname));
        }

        private Microsoft.Maui.Graphics.Color ConvertToMauiColor(System.Drawing.Color systemColor)
        {
            float red = systemColor.R / 255f;
            float green = systemColor.G / 255f;
            float blue = systemColor.B / 255f;
            float alpha = systemColor.A / 255f;

            return new Microsoft.Maui.Graphics.Color(red, green, blue, alpha);
        }
    }
}
