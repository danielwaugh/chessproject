﻿using System;
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
            ;
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

        private void NewGame(object sender, RoutedEventArgs e)
        {
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
            getGrid().Children.Remove(startMenu);
        }

        private void MainMenu(object sender, RoutedEventArgs e)
        {
            getGrid().Children.Remove(popUpMenu);
            getGrid().Children.Add(startMenu);
            startMenu.Opacity = 1;
        }

        private void SaveGame(object sender, RoutedEventArgs e)
        {
            savedGame.Save(chessBoard);
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
            for(int i = 1; i < 9; i++)
            {
                Button curPiece = (Button)this.FindName($"p{i}");
                Grid parent = (Grid)curPiece.Parent;
                if(parent.Name == "maingrid")
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
