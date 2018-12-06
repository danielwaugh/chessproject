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
        Board chessBoard = new Board();     //create new board object

        bool selectFlag = true;     //for selecting pieces. if it is true, it means no piece is selected. if it is false, the piece is not selected

        public MainPage()
        {
            this.InitializeComponent();     //initializes all the xaml objects and pre work for app
            UIGlobal.XAMLpage = this;       //for use of making xaml objects public for other classes
        }

        public Grid getGrid()
        {
            return maingrid;        //returns the grid, so that the chess pieces can be used in external classes
        }
        private void SelectPiece(object sender, RoutedEventArgs e)      //button action for all the pieces
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
    }
}
