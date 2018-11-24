using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Chess
{
    class Board
    {
        private int[,] piecePositions = new int[8, 8]       //holds the position of all the pieces on the 8x8 board
            { {1, 2, 3, 4, 5, 6, 7, 8}, {9, 10, 11, 12, 13, 14, 15, 16}, {0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0}, {17, 18, 19, 20, 21, 22, 23, 24}, {25, 26, 27, 28, 29, 30, 31, 32} };

        public int[,] selectedPiece = new int[8, 8];         //holds the position of the currently selected piece

        private int[,] emptyLocations = new int[8, 8];         //holds the position of the currently selected piece

        private List<Button> emptyButtons = new List<Button>();

        private Button currentPiece = new Button();

        public bool finishedMoveFlag = false;

        private int turn;       //holds whos turn it is


        public Board()
        {
            turn = 0;
        }

        public void SelectPiece(int pieceNumber, Button piece)
        {
            finishedMoveFlag = false;
            currentPiece = piece;
            for (int i = 0; i < 8; i++) //Nested for loop to find selected piece location
            {
                for (int j = 0; j < 8; j++)
                {
                    if (piecePositions[i, j] == pieceNumber)
                    {
                        selectedPiece[i, j] = 1;
                    }
                }
            }
            if (pieceNumber == 1 || pieceNumber == 8 || pieceNumber == 25 || pieceNumber == 32) //Checks if piece is Rook
            {
                Rook thisRook = new Rook(selectedPiece); //Creates new rook object
                thisRook.createDestination(); //Destination array created 
                for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for Rook Movement
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (thisRook.validMoveLocations[i,j] == true)
                        {
                            Button curButton = new Button();
                            curButton.Width = 72;
                            curButton.Height = 72;
                            curButton.Name = $"{i + 1},{j + 1}";
                            Grid.SetRow(curButton, i + 1);
                            Grid.SetColumn(curButton, j + 1);
                            curButton.Click += Move;
                            UIGlobal.XAMLpage.getGrid().Children.Add(curButton);
                            emptyLocations[i, j] = 1;
                            emptyButtons.Add(curButton);
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (piecePositions[i, j] == 0)
                        {
                            Button curButton = new Button();
                            curButton.Width = 72;
                            curButton.Height = 72;
                            curButton.Name = $"{i + 1},{j + 1}";
                            Grid.SetRow(curButton, i + 1);
                            Grid.SetColumn(curButton, j + 1);
                            curButton.Click += Move;
                            UIGlobal.XAMLpage.getGrid().Children.Add(curButton);
                            emptyLocations[i, j] = 1;
                            emptyButtons.Add(curButton);
                        }
                        if (piecePositions[i, j] == pieceNumber)
                        {
                            selectedPiece[i, j] = 1;
                        }
                    }
                }
            }
            
        }

        public void UnselectPiece(int pieceNumber)
        {
            foreach(Button clearbutton in emptyButtons)
            {
                UIGlobal.XAMLpage.getGrid().Children.Remove(clearbutton);
            }
            Array.Clear(emptyLocations, 0, emptyLocations.Length);
            Array.Clear(selectedPiece, 0, selectedPiece.Length);
            emptyButtons.Clear();
        }

        private void Move(object sender, RoutedEventArgs e)
        {
            int curPieceNumber = Convert.ToInt32(currentPiece.Name.Substring(1));

            Button curButton = (Button)sender;
            string[] rowCol = curButton.Name.Split(',');

            Grid.SetRow(currentPiece, Convert.ToInt32(rowCol[0]));
            Grid.SetColumn(currentPiece, Convert.ToInt32(rowCol[1]));

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (piecePositions[i, j] == curPieceNumber)
                    {
                        piecePositions[i, j] = 0;
                    }
                }
            }
            piecePositions[Convert.ToInt32(rowCol[0]) - 1, Convert.ToInt32(rowCol[1]) - 1] = curPieceNumber;
            UnselectPiece(curPieceNumber);
            finishedMoveFlag = true;
            ChangeTurn();
        }

        public void ChangeTurn()
        {
            if (turn == 0)
            {
                for(int i = 1; i < 17; i++)
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;
                    curButton.IsEnabled = false;
                    curButton.Opacity = .7;
                }
                for(int i = 17; i < 33; i++)
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;
                    curButton.IsEnabled = true;
                    curButton.Opacity = 1;
                }
                turn = 1;
            }
            else
            {
                for (int i = 1; i < 17; i++)
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;
                    curButton.IsEnabled = true;
                    curButton.Opacity = 1;
                }
                for (int i = 17; i < 33; i++)
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;
                    curButton.IsEnabled = false;
                    curButton.Opacity = .7;
                }
                turn = 0;
            }
        }
    }
}
