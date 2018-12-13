using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Media.Imaging;

namespace Chess
{
    public sealed partial class MainPage : Page
    {
        private Board chessBoard;     //create new board object

        private SaveState savedGame; //saved board is stored here

        bool selectFlag = true;     //for selecting pieces. if it is true, it means no piece is selected. if it is false, the piece is not selected

        Grid startMenu = new Grid();

        Grid popUpMenu = new Grid();

        public MainPage()
        {
            this.InitializeComponent();     //initializes all the xaml objects and pre work for app
            UIGlobal.XAMLpage = this;       //for use of making xaml objects public for other classes
            popUpMenu = (Grid)this.FindName("gamepopupmenu");
            this.getGrid().Children.Remove(popUpMenu);
            savedGame = new SaveState();
        }

        public Grid getGrid()
        {
            return maingrid;        //returns the grid, so that the chess pieces can be used in external classes
        }

        public void SelectPiece(object sender, RoutedEventArgs e)      //button action for all the pieces
        {
            Button currentPieceButton = (Button)sender; //cast the object to a button type
            int currentPieceNumber = 0;     //to store current pieces number
            currentPieceNumber = Convert.ToInt32(currentPieceButton.Name.Substring(1)); //sets the current piece number to the xaml name (representing the numbered piece)

            if (selectFlag || chessBoard.finishedMoveFlag)      //if a piece is not selected or a move just finished
            {
                chessBoard.SelectPiece(currentPieceNumber, currentPieceButton);     //run the board functionn that selects the piece and creates locations it can go
                selectFlag = false;     //tells the program that a piece has been selected
            }
            else
            {
                chessBoard.UnselectPiece(currentPieceNumber);       //unselects piece only if it is already selected
                selectFlag = true;      //tells the program that the piece has been unselected
            }
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            if (sender == null)
            {
                throw new ArgumentNullException(nameof(sender));
            }

            startMenu = (Grid)this.FindName("startmenu");
            this.getGrid().Children.Remove(startMenu);
            TextBlock player1text = (TextBlock)this.FindName("player1captured");
            player1text.Opacity = 1;
            TextBlock player2text = (TextBlock)this.FindName("player2captured");
            player2text.Opacity = 1;
            chessBoard = new Board();
            RearangePieces();
        }

        private void LoadGame(object sender, RoutedEventArgs e)
        {
            chessBoard = null;
            chessBoard = savedGame.Load();
            RearangePieces();
            TextBlock player1text = (TextBlock)this.FindName("player1captured");
            player1text.Opacity = 1;
            TextBlock player2text = (TextBlock)this.FindName("player2captured");
            player2text.Opacity = 1;
            TextBlock autoSave = (TextBlock)this.FindName("autosave");
            autoSave.Opacity = 1;
            RearangePiecesLoad(chessBoard);
            getGrid().Children.Remove(startMenu);
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            TextBlock autoSave = (TextBlock)this.FindName("autosave");
            autoSave.Opacity = 0;
            getGrid().Children.Remove(popUpMenu);
            getGrid().Children.Add(startMenu);
            startMenu.Opacity = 1;
        }

        private void SaveGame(object sender, RoutedEventArgs e)
        {
            chessBoard.UnselectPiece(chessBoard.GetCurrentPieceNumber());
            savedGame.Save(chessBoard);
            TextBlock autoSave = (TextBlock)this.FindName("autosave");
            autoSave.Opacity = 1;
        }

        private void ReturnToGame(object sender, RoutedEventArgs e)
        {
            TextBlock player1text = (TextBlock)this.FindName("player1captured");
            player1text.Opacity = 1;
            TextBlock player2text = (TextBlock)this.FindName("player2captured");
            player2text.Opacity = 1;
            getGrid().Children.Remove(popUpMenu);
        }

        private void GameMenu(object sender, RoutedEventArgs e)
        {
            TextBlock player1text = (TextBlock)this.FindName("player1captured");
            player1text.Opacity = 0;
            TextBlock player2text = (TextBlock)this.FindName("player2captured");
            player2text.Opacity = 0;
            getGrid().Children.Add(popUpMenu);
            popUpMenu.Opacity = 1;
        }

        private void RearangePieces()
        {
            for (int i = 1; i < 9; i++)
            {
                Button curPiece = (Button)this.FindName($"p{i}");
                Grid parent = (Grid)curPiece.Parent;
                if (parent.Name == "maingrid")
                {
                    Grid.SetRow(curPiece, 1);
                    Grid.SetColumn(curPiece, i);
                }
                else if (parent.Name == "player1grid")
                {
                    UIGlobal.getPlayer1Grid().Children.Remove(curPiece);
                    getGrid().Children.Add(curPiece);
                    Grid.SetRow(curPiece, 1);
                    Grid.SetColumn(curPiece, i);
                }
                else if (parent.Name == "player2grid")
                {
                    UIGlobal.getPlayer2Grid().Children.Remove(curPiece);
                    getGrid().Children.Add(curPiece);
                    Grid.SetRow(curPiece, 1);
                    Grid.SetColumn(curPiece, i);
                }
                curPiece.IsEnabled = true;
                curPiece.Opacity = 1;
            }
            for (int i = 9, j = 1; j < 9 && i < 17; i++, j++)
            {
                Button curPiece = (Button)this.FindName($"p{i}");
                Grid parent = (Grid)curPiece.Parent;
                if (parent.Name == "maingrid")
                {
                    Grid.SetRow(curPiece, 2);
                    Grid.SetColumn(curPiece, j);
                }
                else if (parent.Name == "player1grid")
                {
                    UIGlobal.getPlayer1Grid().Children.Remove(curPiece);
                    getGrid().Children.Add(curPiece);
                    Grid.SetRow(curPiece, 2);
                    Grid.SetColumn(curPiece, j);
                }
                else if (parent.Name == "player2grid")
                {
                    UIGlobal.getPlayer2Grid().Children.Remove(curPiece);
                    getGrid().Children.Add(curPiece);
                    Grid.SetRow(curPiece, 2);
                    Grid.SetColumn(curPiece, j);
                }
                curPiece.IsEnabled = true;
                curPiece.Opacity = 1;
            }
            for (int i = 17, j = 1; j < 9 && i < 25; i++, j++)
            {
                Button curPiece = (Button)this.FindName($"p{i}");
                Grid parent = (Grid)curPiece.Parent;
                if (parent.Name == "maingrid")
                {
                    Grid.SetRow(curPiece, 7);
                    Grid.SetColumn(curPiece, j);
                }
                else if (parent.Name == "player1grid")
                {
                    UIGlobal.getPlayer1Grid().Children.Remove(curPiece);
                    getGrid().Children.Add(curPiece);
                    Grid.SetRow(curPiece, 7);
                    Grid.SetColumn(curPiece, j);
                }
                else if (parent.Name == "player2grid")
                {
                    UIGlobal.getPlayer2Grid().Children.Remove(curPiece);
                    getGrid().Children.Add(curPiece);
                    Grid.SetRow(curPiece, 7);
                    Grid.SetColumn(curPiece, j);
                }
                curPiece.IsEnabled = false;
                curPiece.Opacity = 0.7;
            }
            for (int i = 25, j = 1; j < 9 && i < 33; i++, j++)
            {
                Button curPiece = (Button)this.FindName($"p{i}");
                Grid parent = (Grid)curPiece.Parent;
                if (parent.Name == "maingrid")
                {
                    Grid.SetRow(curPiece, 8);
                    Grid.SetColumn(curPiece, j);
                }
                else if (parent.Name == "player1grid")
                {
                    UIGlobal.getPlayer1Grid().Children.Remove(curPiece);
                    getGrid().Children.Add(curPiece);
                    Grid.SetRow(curPiece, 8);
                    Grid.SetColumn(curPiece, j);
                }
                else if (parent.Name == "player2grid")
                {
                    UIGlobal.getPlayer2Grid().Children.Remove(curPiece);
                    getGrid().Children.Add(curPiece);
                    Grid.SetRow(curPiece, 8);
                    Grid.SetColumn(curPiece, j);
                }
                curPiece.IsEnabled = false;
                curPiece.Opacity = 0.7;
            }
        }

        private void RearangePiecesLoad(Board curBoard)
        {
            int[,] piecePositions = curBoard.GetPiecePositions();

            int curPieceNumber = 0;

            int[] blackCaptured = new int[2] { 1, 1 };

            int[] whiteCaptured = new int[2] { 1, 1 };

            for (int i = 0; i < 8; i++)
            {
                for (int k = 0; k < 8; k++)
                {
                    curPieceNumber = piecePositions[i, k];
                    if (curPieceNumber == 0)
                    {
                        continue;
                    }
                    else
                    {
                        Button curPiece = (Button)this.FindName($"p{curPieceNumber}");  //need to check if it is in grid or in captured grid
                        Grid.SetRow(curPiece, i + 1);
                        Grid.SetColumn(curPiece, k + 1);
                    }
                }
            }

            foreach (int piece in curBoard.GetCapturedPieces())
            {
                Button curPiece = (Button)this.FindName($"p{piece}");
                if (piece < 17)
                {
                    Grid.SetRow(curPiece, whiteCaptured[0]);
                    Grid.SetColumn(curPiece, whiteCaptured[1]);
                    getGrid().Children.Remove(curPiece);
                    UIGlobal.getPlayer2Grid().Children.Add(curPiece);
                    if (whiteCaptured[1] == 2) //increments grid location 
                    {
                        whiteCaptured[0]++;
                        whiteCaptured[1] = 1;
                    }
                    else
                    {
                        whiteCaptured[1]++;
                    }
                }
                else
                {
                    Grid.SetRow(curPiece, blackCaptured[0]);
                    Grid.SetColumn(curPiece, blackCaptured[1]);
                    getGrid().Children.Remove(curPiece);
                    UIGlobal.getPlayer1Grid().Children.Add(curPiece);
                    if (blackCaptured[1] == 2) //increments grid location 
                    {
                        blackCaptured[0]++;
                        blackCaptured[1] = 1;
                    }
                    else
                    {
                        blackCaptured[1]++;
                    }
                }
            }
            if (chessBoard.GetTurn() == 0)  //turn = 0 is white, turn = 1 is black
            {
                for (int i = 1; i < 17; i++)    //sets all the white buttons to an opacity of 1 and enables the buttons
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;   //gets piece by name
                    if (!curButton.IsEnabled && curButton.Opacity == 1) //checks to see if it is in the captured bank
                    {
                        continue;
                    }
                    curButton.IsEnabled = true;
                    curButton.Opacity = 1;
                }
                for (int i = 17; i < 33; i++)   //sets all the black buttons to an opacity of .7 and disables the buttons
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;   //gets piece by name
                    if (!curButton.IsEnabled && curButton.Opacity == 1) //checks to see if it is in the captured bank
                    {
                        continue;
                    }
                    curButton.IsEnabled = false;
                    curButton.Opacity = .7;
                }
            }
            else
            {
                for (int i = 1; i < 17; i++)     //sets all the white buttons to an opacity of .7 and disables the buttons
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;   //gets piece by name
                    if (!curButton.IsEnabled && curButton.Opacity == 1)  //checks to see if it is in the captured bank
                    {
                        continue;
                    }
                    curButton.IsEnabled = false;
                    curButton.Opacity = .7;
                }
                for (int i = 17; i < 33; i++)    //sets all the black buttons to an opacity of 1 and enables the buttons
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;   //gets piece by name
                    if (!curButton.IsEnabled && curButton.Opacity == 1) //checks to see if it is in the captured bank
                    {
                        continue;
                    }
                    curButton.IsEnabled = true;
                    curButton.Opacity = 1;
                }
            }
        }
    }

    /// <summary>
    /// This class handles making the grid public to other classes. Without this,
    /// it would not be possible to modify the xaml file outside of the xaml main
    /// class. It is static (unchanging) and returns the grid of the xaml class. 
    /// </summary>
    public static class UIGlobal
    {
        public static MainPage XAMLpage { get; set; }       //create the main page object

        public static Grid getGrid()
        {
            return XAMLpage.getGrid();  //returns the grid object to be used in external classes //OfType<Grid>().
        }

        public static Grid getPlayer1Grid()
        {
            return (Grid)XAMLpage.FindName("player1grid");
        }

        public static Grid getPlayer2Grid()
        {
            return (Grid)XAMLpage.FindName("player2grid");
        }

        public static void RunSelectPieceOnce(Button curButton, RoutedEventArgs e)
        {
            XAMLpage.SelectPiece(curButton, e);
        }
    }
}

