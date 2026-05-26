using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace PyramidSystem
{
    public class Row 
    {

        public List<LetterTile> LetterTiles { get; set; } = new();
        public bool IsCorrect { get; set; }


        public bool DetectCompleteRow()
        {
            return LetterTiles.All(l => l.LetterTileValue != "");
        }
        public void clear()
        {
            foreach (LetterTile l in LetterTiles)
            {
                l.LetterTileValue = "";
            }
        }
    }
}
