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
        Board chessBoard = new Board();

        bool selectFlag = true;

        public MainPage()
        {
            this.InitializeComponent();
            UIGlobal.XAMLpage = this;
        }

        public Grid getGrid()
        {
            return maingrid;
        }
        private void SelectPiece(object sender, RoutedEventArgs e)
        {
            Button currentPieceButton = (Button)sender;
            int currentPieceNumber = 0;
            currentPieceNumber = Convert.ToInt32(currentPieceButton.Name.Substring(1));

            if (selectFlag || chessBoard.finishedMoveFlag)
            {
                chessBoard.SelectPiece(currentPieceNumber, currentPieceButton);
                selectFlag = false;
            }
            else
            {
                chessBoard.UnselectPiece(currentPieceNumber);
                selectFlag = true;
            }


        }

        /*private void PawnSingleMoveWhite(object sender, RoutedEventArgs e)
        {
            Button curButton = sender as Button;
            int currentRow = Grid.GetRow(curButton);
            Grid.SetRow(curButton, ++currentRow);
        }*/

    }
    public static class UIGlobal
    {
        public static MainPage XAMLpage { get; set; }

        public static Grid getGrid()
        {
            return XAMLpage.getGrid();
        }
    }
}
