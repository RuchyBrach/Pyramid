using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using PyramidSystem;
namespace PyramidTest
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestStartGame()
        {
            Game game = new();
            game.StartGame();
            string startletter = game.Rows[0].LetterTiles[0].LetterTileValue;
            bool b = true;
            for (int i = 1; i < game.Rows.Count; i++)
            {
                foreach (LetterTile l in game.Rows[i].LetterTiles)
                {
                    if (l.LetterTileValue != "")
                    {
                        b = false;
                    }
                }
            }
            string msg = $"startletter = '{startletter}' remainingtilesblank = {b} score = {game.Score}";
            Assert.IsTrue(startletter != "" && b == true && game.Score == 0, msg);
            TestContext.Write(msg);
        }
        [Test]
        public void TestDetectword()
        {
            Game game = new();
            game.StartGame();
            string s = game.Rows[0].LetterTiles[0].LetterTileValue = "T";
            Row row = game.Rows[1];
            row.LetterTiles[0].LetterTileValue = s;
            row.LetterTiles[1].LetterTileValue = "O";
            string word = $"{game.Rows[1].LetterTiles[0].LetterTileValue}{game.Rows[1].LetterTiles[1].LetterTileValue}";
            bool b = game.DetectWord(row);
            string msg = $"Word = {word} valid: {b}";
            Assert.IsTrue(b == true, msg);
            TestContext.Write(msg);
        }
        [Test]
        public void TestContainsPreviousWordFalse()
        {
            Game game = new();
            game.StartGame();
            game.Rows[0].LetterTiles[0].LetterTileValue = "T";
            Row row = game.Rows[1];
            row.LetterTiles[0].LetterTileValue = "H";
            row.LetterTiles[1].LetterTileValue = "I";
            bool b = game.ContainsPreviousWord(row, "HI");
            string msg = $"contains previous word: {b}";
            Assert.IsTrue(b == false, msg);
            TestContext.WriteLine(msg);
        }
        [Test]
        public void TestRemainingAttempts()
        {
            Game game = new();
            game.StartGame();
            game.Rows[0].LetterTiles[0].LetterTileValue = "T";
            Row row = game.Rows[1];
            row.LetterTiles[0].LetterTileValue = "H";
            row.LetterTiles[1].LetterTileValue = "I";
            game.DetectWord(row);
            string msg = $"current remaining attempts = {game.RemainingAttempts}. remaining attempts should be 2";
            Assert.IsTrue(game.RemainingAttempts == 2, msg);
            TestContext.WriteLine(msg);
        }
        [Test]
        public void TestScoreIncrease()
        {
            Game game = new();
            game.StartGame();
            Row row4 = game.Rows[3];
            row4.LetterTiles[0].LetterTileValue = "S";
            row4.LetterTiles[1].LetterTileValue = "T";
            row4.LetterTiles[2].LetterTileValue = "O";
            row4.LetterTiles[3].LetterTileValue = "N";
                                                   
            Row row5 = game.Rows[4];              
            row5.LetterTiles[0].LetterTileValue = "S";
            row5.LetterTiles[1].LetterTileValue = "T";
            row5.LetterTiles[2].LetterTileValue = "O";
            row5.LetterTiles[3].LetterTileValue = "N";
            row5.LetterTiles[4].LetterTileValue = "E";
            game.DetectWord(row5);

            string msg = $"Score = {game.Score}. Expected score = 1";
            Assert.IsTrue(game.Score == 1, msg);
            TestContext.WriteLine(msg);
        }
        [Test]
        public void TestGetNewPyramid()
        {
            Game game = new();
            game.StartGame();
            Row row4 = game.Rows[3];
            row4.LetterTiles[0].LetterTileValue = "S";
            row4.LetterTiles[1].LetterTileValue = "T";
            row4.LetterTiles[2].LetterTileValue = "O";
            row4.LetterTiles[3].LetterTileValue = "N";
                                                  
            Row row5 = game.Rows[4];              
            row5.LetterTiles[0].LetterTileValue = "S";
            row5.LetterTiles[1].LetterTileValue = "T";
            row5.LetterTiles[2].LetterTileValue = "O";
            row5.LetterTiles[3].LetterTileValue = "N";
            row5.LetterTiles[4].LetterTileValue = "E";
            game.DetectWord(row5);
            bool b = true;
            for (int i = 1; i < game.Rows.Count; i++)
            {
                foreach (LetterTile l in game.Rows[i].LetterTiles)
                {
                    if (l.LetterTileValue != "")
                    {
                        b = false;
                    }
                }
            }

            string msg = $"all lettertiles cleared = {b}. expected lettertiles cleared = true";
            Assert.IsTrue(b == true, msg);
            TestContext.WriteLine(msg);
        }
        [Test]
        public void TestDetectCompleteRow()
        {
            Game game = new();
            game.StartGame();
            Row row = game.Rows[1];
            row.LetterTiles[0].LetterTileValue = "T";
            row.LetterTiles[1].LetterTileValue = "O";

            bool b = row.DetectCompleteRow();

            string msg =
                $"row complete = {b}. expcted row complete = true";

            Assert.IsTrue(b == true, msg);

            TestContext.WriteLine(msg);

        }
        [Test]
        public void TestGameOver()
        {
            Game game = new();
            game.StartGame();
            Row row2 = game.Rows[1];
            row2.LetterTiles[0].LetterTileValue = "S";
            row2.LetterTiles[1].LetterTileValue = "H";
            Row row3 = game.Rows[2];
            row3.LetterTiles[0].LetterTileValue = "S";
            row3.LetterTiles[1].LetterTileValue = "H";
            row3.LetterTiles[2].LetterTileValue = "H";
            game.DetectWord(row3);               
            row3.LetterTiles[0].LetterTileValue = "S";
            row3.LetterTiles[1].LetterTileValue = "T";
            row3.LetterTiles[2].LetterTileValue = "O";
            game.DetectWord(row3);               
            row3.LetterTiles[0].LetterTileValue = "S";
            row3.LetterTiles[1].LetterTileValue = "T";
            row3.LetterTiles[2].LetterTileValue = "R";
            game.DetectWord(row3);
            string msg = $"remaining attempts = {game.RemainingAttempts}. message = {game.Message}.";
            Assert.IsTrue(game.RemainingAttempts == 0 && game.Message == "Game Over", msg);
            TestContext.WriteLine(msg);
        }
    }
}