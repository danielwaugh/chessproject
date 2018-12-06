using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Newtonsoft.Json;

namespace Chess
{
    class Board
    {
        /// <summary>
        /// holds the position of all the pieces on the 8x8 board (total of 32 pieces represented by corresponding
        /// numbers, 0 represents empty position). These numbers map to the name of the xaml object in the uwp application
        /// </summary>
        private int[,] piecePositions = new int[8, 8]      
            { {1, 2, 3, 4, 5, 6, 7, 8}, {9, 10, 11, 12, 13, 14, 15, 16}, {0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, 0}, {0, 0, 0, 0, 0, 0, 0, 0}, {17, 18, 19, 20, 21, 22, 23, 24}, {25, 26, 27, 28, 29, 30, 31, 32} };

        public int[,] selectedPiece = new int[8, 8];         //holds the position of the currently selected piece

        private List<Button> createdButtons = new List<Button>();   //holds a list of the buttons created when a piece is selected (desitination buttons)

        private Button currentPiece = new Button(); //holds the current piece that is selected

        public bool finishedMoveFlag = false;   //set when a move is finished

        private int turn;       //holds whos turn it is


        public Board()
        {
            turn = 0;   //turn starts out with white pieces
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
                        selectedPiece[i, j] = 1;    //puts the location of the selected piece in the grid array
                    }
                }
            }
            if (pieceNumber == 1 || pieceNumber == 8 || pieceNumber == 25 || pieceNumber == 32) //Checks if piece is Rook
            {
                Rook thisRook = new Rook(selectedPiece); //Creates new rook object
                thisRook.createDestination(piecePositions, turn); //Destination array created 
                for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for Rook Movement
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (thisRook.validMoveLocations[i,j] == true)
                        {
                            Button curButton = new Button();    //creates button
                            curButton.Width = 72;
                            curButton.Height = 72;
                            curButton.Name = $"{i + 1},{j + 1}";    //gives it a name for indexing later (name: "grid row, grid column")
                            Grid.SetRow(curButton, i + 1);  //sets the location in the grid
                            Grid.SetColumn(curButton, j + 1);
                            curButton.Click += Move;    //sets method to run when button is pressed
                            UIGlobal.XAMLpage.getGrid().Children.Add(curButton);    //adds button to grid 
                            createdButtons.Add(curButton);
                        }
                    }
                }
            }

            else if (pieceNumber == 3 || pieceNumber == 6 || pieceNumber == 27 || pieceNumber == 30) //Checks if piece is Bishop
            {
                Bishop thisBishop = new Bishop(selectedPiece); //Creates new Bishop object
                thisBishop.createDestination(piecePositions, turn); //Destination array created 
                for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for Bishop Movement
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (thisBishop.validMoveLocations[i, j] == true)
                        {
                            Button curButton = new Button();    //creates button
                            curButton.Width = 72;
                            curButton.Height = 72;
                            curButton.Name = $"{i + 1},{j + 1}";    //gives it a name for indexing later (name: "grid row, grid column")
                            Grid.SetRow(curButton, i + 1);  //sets the location in the grid
                            Grid.SetColumn(curButton, j + 1);
                            curButton.Click += Move;    //sets method to run when button is pressed
                            UIGlobal.XAMLpage.getGrid().Children.Add(curButton);     //adds button to grid 
                            createdButtons.Add(curButton);
                        }
                    }
                }
            }

            else if (pieceNumber == 4 || pieceNumber == 28) //Checks if piece is Queen
            {
                Queen thisQueen = new Queen(selectedPiece); //Creates new Queen object
                thisQueen.createDestination(piecePositions, turn); //Destination array created 
                for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for Queen Movement
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (thisQueen.validMoveLocations[i, j] == true)
                        {
                            Button curButton = new Button();    //creates button
                            curButton.Width = 72;
                            curButton.Height = 72;
                            curButton.Name = $"{i + 1},{j + 1}";    //gives it a name for indexing later (name: "grid row, grid column")
                            Grid.SetRow(curButton, i + 1);  //sets the location in the grid
                            Grid.SetColumn(curButton, j + 1);
                            curButton.Click += Move;    //sets method to run when button is pressed
                            UIGlobal.XAMLpage.getGrid().Children.Add(curButton);     //adds button to grid 
                            createdButtons.Add(curButton);
                        }
                    }
                }
            }
            else if (pieceNumber == 2 || pieceNumber == 7 || pieceNumber == 26 || pieceNumber == 31) //Checks if piece is Knight
            {
                Knight thisKnight = new Knight(selectedPiece); //Creates new Queen object
                thisKnight.createDestination(piecePositions, turn); //Destination array created 
                for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for Queen Movement
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (thisKnight.validMoveLocations[i, j] == true)
                        {
                            Button curButton = new Button();    //creates button
                            curButton.Width = 72;
                            curButton.Height = 72;
                            curButton.Name = $"{i + 1},{j + 1}";    //gives it a name for indexing later (name: "grid row, grid column")
                            Grid.SetRow(curButton, i + 1);  //sets the location in the grid
                            Grid.SetColumn(curButton, j + 1);
                            curButton.Click += Move;    //sets method to run when button is pressed
                            UIGlobal.XAMLpage.getGrid().Children.Add(curButton);     //adds button to grid 
                            createdButtons.Add(curButton);
                        }
                    }
                }
            }
            else if (pieceNumber == 5 || pieceNumber == 29) //Checks if piece is King
            {
                King thisKing = new King(selectedPiece); //Creates new King object
                thisKing.createDestination(piecePositions, turn); //Destination array created 
                for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for King Movement
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (thisKing.validMoveLocations[i, j] == true)
                        {
                            Button curButton = new Button();    //creates button
                            curButton.Width = 72;
                            curButton.Height = 72;
                            curButton.Name = $"{i + 1},{j + 1}";    //gives it a name for indexing later (name: "grid row, grid column")
                            Grid.SetRow(curButton, i + 1);  //sets the location in the grid
                            Grid.SetColumn(curButton, j + 1);
                            curButton.Click += Move;    //sets method to run when button is pressed
                            UIGlobal.XAMLpage.getGrid().Children.Add(curButton);     //adds button to grid 
                            createdButtons.Add(curButton);
                        }
                    }
                }
            }
            else
            {
                Pawn thisPawn = new Pawn(selectedPiece); //Creates new Pawn object
                thisPawn.createDestination(piecePositions, turn); //Destination array created 
                for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for Pawn Movement
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (thisPawn.validMoveLocations[i, j] == true)
                        {
                            Button curButton = new Button();    //creates button
                            curButton.Width = 72;
                            curButton.Height = 72;
                            curButton.Name = $"{i + 1},{j + 1}";    //gives it a name for indexing later (name: "grid row, grid column")
                            Grid.SetRow(curButton, i + 1);  //sets the location in the grid
                            Grid.SetColumn(curButton, j + 1);
                            curButton.Click += Move;    //sets method to run when button is pressed
                            UIGlobal.XAMLpage.getGrid().Children.Add(curButton);     //adds button to grid 
                            createdButtons.Add(curButton);
                        }
                    }
                }
            }
            
        }

        /// <summary>
        /// unselects the piece by deleting all buttons created by the select piece function
        /// </summary>
        /// <param name="pieceNumber"></param>
        public void UnselectPiece(int pieceNumber)  
        {
            foreach(Button clearbutton in createdButtons)     //goes through emptybutton locations (places where the piece can go)
            {
                UIGlobal.XAMLpage.getGrid().Children.Remove(clearbutton);       //removes the buttons from the board
            }
            Array.Clear(selectedPiece, 0, selectedPiece.Length);        //clears the aray of location of selected piece
            createdButtons.Clear();     //clears the createdButtons list (so that it can be used again)
        }

        /// <summary>
        /// Moves the piece button from the current location to the place wehre the 
        /// user selected it to move. This occurs when any of the created Buttons are pressed
        /// </summary>
        private void Move(object sender, RoutedEventArgs e)
        {
            int curPieceNumber = Convert.ToInt32(currentPiece.Name.Substring(1));   //gets the current pieces identification number for xaml

            Button curButton = (Button)sender;      //casts the object to button
            string[] rowCol = curButton.Name.Split(',');    //splits the name of the created button to get the row and column of it's location

            Grid.SetRow(currentPiece, Convert.ToInt32(rowCol[0]));  //sets the row of the current piece selected to row of button pressed
            Grid.SetColumn(currentPiece, Convert.ToInt32(rowCol[1]));   //sets the col of the current piece selected to col of button pressed

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (piecePositions[i, j] == curPieceNumber)     //clears the piece positions in the back end grid of where the piece used to be
                    {
                        piecePositions[i, j] = 0;
                    }
                }
            }
            piecePositions[Convert.ToInt32(rowCol[0]) - 1, Convert.ToInt32(rowCol[1]) - 1] = curPieceNumber;    //stores the new location of the selected piece
            UnselectPiece(curPieceNumber);  //unselects the piece by using the unselect function
            finishedMoveFlag = true;    //move is now finished
            ChangeTurn();       //changes the turn to the other player
        }

        /// <summary>
        /// handles changing the turn so only one users pieces are usable at a time
        /// </summary>
        public void ChangeTurn()
        {
            if (turn == 0)  //turn = 0 is white, turn = 1 is black
            {
                for(int i = 1; i < 17; i++)     //sets all the white buttons to an opacity of .7 and disables the buttons
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;   //gets piece by name
                    curButton.IsEnabled = false;
                    curButton.Opacity = .7;
                }
                for(int i = 17; i < 33; i++)    //sets all the black buttons to an opacity of 1 and enables the buttons
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;   //gets piece by name
                    curButton.IsEnabled = true;
                    curButton.Opacity = 1;
                }
                turn = 1;   //changes turn
            }
            else
            {
                for (int i = 1; i < 17; i++)    //sets all the white buttons to an opacity of 1 and enables the buttons
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;   //gets piece by name
                    curButton.IsEnabled = true;
                    curButton.Opacity = 1;
                }
                for (int i = 17; i < 33; i++)   //sets all the black buttons to an opacity of .7 and disables the buttons
                {
                    Button curButton = UIGlobal.XAMLpage.FindName($"p{i}") as Button;   //gets piece by name
                    curButton.IsEnabled = false;
                    curButton.Opacity = .7;
                }
                turn = 0;   //changees turn
            }
        }
    }
}

//UIGlobal.getGrid().Children.Remove();
//UIGlobal.getPlayer1Grid().Children.Add();
