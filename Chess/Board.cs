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

        private int[] capturedPieceWhite = new int[2] { 1, 1 };

        private int[] capturedPieceBlack = new int[2] { 1, 1 };

        private List<int> capturedList = new List<int>();

        private bool checkmode = false;

        private bool[,] checkModePath = new bool[8, 8];

        private bool[,] validKingMovements = new bool[8, 8];

        /// <summary>
        /// Constructor
        /// </summary>
        public Board()
        {
            turn = 0;   //turn starts out with white pieces
        }

        /// <summary>
        /// moves the appropriate captured piece from the board to the captured bank
        /// of the specefied player.
        /// </summary>
        /// <param name="piece"></param>
        private void CapturePiece(int piece)
        {
            Button captureButton = (Button)UIGlobal.XAMLpage.FindName($"p{piece}");
            if (turn == 0)  //whites turn
            {
                Grid.SetRow(captureButton, capturedPieceWhite[0]);
                Grid.SetColumn(captureButton, capturedPieceWhite[1]);
                captureButton.IsEnabled = false;
                captureButton.Opacity = 1;
                UIGlobal.getGrid().Children.Remove(captureButton);
                UIGlobal.getPlayer1Grid().Children.Add(captureButton);
                if (capturedPieceWhite[1] == 2)
                {
                    capturedPieceWhite[0]++;
                    capturedPieceWhite[1] = 1;
                }
                else
                {
                    capturedPieceWhite[1]++;
                }
            }
            if (turn == 1)  //blacks turn
            {
                Grid.SetRow(captureButton, capturedPieceBlack[0]);
                Grid.SetColumn(captureButton, capturedPieceBlack[1]);
                captureButton.IsEnabled = false;
                captureButton.Opacity = 1;
                UIGlobal.getGrid().Children.Remove(captureButton);
                UIGlobal.getPlayer2Grid().Children.Add(captureButton);
                if (capturedPieceBlack[1] == 2)
                {
                    capturedPieceBlack[0]++;
                    capturedPieceBlack[1] = 1;
                }
                else
                {
                    capturedPieceBlack[1]++;
                }
            }
            capturedList.Add(piece);    //add to the list of captured pieces

        }

        /// <summary>
        /// selects the piece and creates all the destinations that the piece can go to 
        /// </summary>
        /// <param name="pieceNumber"></param>
        /// <param name="piece"></param>
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
            ValidKingMovements();
            if (checkmode)
            {
                if (pieceNumber == 5 || pieceNumber == 29) //Checks if piece is King
                {
                    King thisKing = new King(selectedPiece); //Creates new King object
                    thisKing.createDestination(piecePositions, turn); //Destination array created 
                    for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for King Movement
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (thisKing.validMoveLocations[i, j] == true && checkModePath[i, j] == false && validKingMovements[i, j] == false)  //can only move to spots that wouldn't keep it in check
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
                else if (pieceNumber == 1 || pieceNumber == 8 || pieceNumber == 25 || pieceNumber == 32) //Checks if piece is Rook
                {
                    Rook thisRook = new Rook(selectedPiece); //Creates new rook object
                    thisRook.createDestination(piecePositions, turn); //Destination array created 
                    for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for Rook Movement
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (thisRook.validMoveLocations[i, j] == true && checkModePath[i, j] == true)   //can move to only spots defending the king
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
                            if (thisBishop.validMoveLocations[i, j] == true && checkModePath[i, j] == true)     //can move to only spots defending the king
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
                            if (thisQueen.validMoveLocations[i, j] == true && checkModePath[i, j] == true)  //can move to only spots defending the king
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
                            if (thisKnight.validMoveLocations[i, j] == true && checkModePath[i, j] == true) //can move to only spots defending the king
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
                            if (thisPawn.validMoveLocations[i, j] == true && checkModePath[i, j] == true)
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
            else
            {
                if (pieceNumber == 1 || pieceNumber == 8 || pieceNumber == 25 || pieceNumber == 32) //Checks if piece is Rook
                {
                    Rook thisRook = new Rook(selectedPiece); //Creates new rook object
                    thisRook.createDestination(piecePositions, turn); //Destination array created 
                    for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for Rook Movement
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (thisRook.validMoveLocations[i, j] == true)
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
                            if (thisKing.validMoveLocations[i, j] == true && validKingMovements[i, j] == false)
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


        }

        /// <summary>
        /// unselects the piece by deleting all buttons created by the select piece function
        /// </summary>
        /// <param name="pieceNumber"></param>
        public void UnselectPiece(int pieceNumber)
        {
            foreach (Button clearbutton in createdButtons)     //goes through emptybutton locations (places where the piece can go)
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

            int possibleCapture = piecePositions[Convert.ToInt32(rowCol[0]) - 1, Convert.ToInt32(rowCol[1]) - 1];

            if (turn == 0)  //whites turn
            {
                if (possibleCapture > 16)    // if white is capturing a black, run capture function
                {
                    CapturePiece(possibleCapture);
                }
            }
            else //blacks turn
            {
                if (possibleCapture <= 16 && possibleCapture > 0)
                {
                    CapturePiece(possibleCapture);  //if black is capturing a white, run capture function
                }
            }

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
            selectedPiece[Convert.ToInt32(rowCol[0]) - 1, Convert.ToInt32(rowCol[1]) - 1] = 1;
            Array.Clear(checkModePath, 0, checkModePath.Length);        //clears the array of the checkmode path
            checkmode = false;
            CheckPopUp();
            kingCheck();
            Array.Clear(validKingMovements, 0, validKingMovements.Length);
            Array.Clear(selectedPiece, 0, selectedPiece.Length);        //clears the aray of location of selected piece
            finishedMoveFlag = true;    //move is now finished
            ChangeTurn();       //changes the turn to the other player
        }

        /// <summary>
        /// handles changing the turn so only one users pieces are usable at a time
        /// </summary>
        private void ChangeTurn()
        {
            if (turn == 0)  //turn = 0 is white, turn = 1 is black
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
                turn = 1;   //changes turn
            }
            else
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
                turn = 0;   //changees turn
            }
        }

        /// <summary>
        /// checks to see if the king is in the check stage.
        /// </summary>
        private void kingCheck()
        {
            int pieceNumber = Convert.ToInt32(currentPiece.Name.Substring(1));   //gets the current pieces identification number for xaml
            if (pieceNumber == 1 || pieceNumber == 8 || pieceNumber == 25 || pieceNumber == 32) //Checks if piece is Rook
            {
                Rook thisRook = new Rook(selectedPiece); //Creates new rook object
                thisRook.createDestination(piecePositions, turn); //Destination array created 
                checkModePath = thisRook.validMoveLocations;    //sets valid locations for piece
            }

            else if (pieceNumber == 3 || pieceNumber == 6 || pieceNumber == 27 || pieceNumber == 30) //Checks if piece is Bishop
            {
                Bishop thisBishop = new Bishop(selectedPiece); //Creates new Bishop object
                thisBishop.createDestination(piecePositions, turn); //Destination array created 
                checkModePath = thisBishop.validMoveLocations;  //sets valid locations for piece
            }

            else if (pieceNumber == 4 || pieceNumber == 28) //Checks if piece is Queen
            {
                Queen thisQueen = new Queen(selectedPiece); //Creates new Queen object
                thisQueen.createDestination(piecePositions, turn); //Destination array created 
                checkModePath = thisQueen.validMoveLocations;   //sets valid locations for piece
            }
            else if (pieceNumber == 2 || pieceNumber == 7 || pieceNumber == 26 || pieceNumber == 31) //Checks if piece is Knight
            {
                Knight thisKnight = new Knight(selectedPiece); //Creates new Queen object
                thisKnight.createDestination(piecePositions, turn); //Destination array created 
                checkModePath = thisKnight.validMoveLocations;  //sets valid locations for piece
            }
            else if (pieceNumber == 5 || pieceNumber == 29) //Checks if piece is King
            {
                King thisKing = new King(selectedPiece); //Creates new King object
                thisKing.createDestination(piecePositions, turn); //Destination array created 
                checkModePath = thisKing.validMoveLocations;    //sets valid locations for piece
            }
            else
            {
                Pawn thisPawn = new Pawn(selectedPiece); //Creates new Pawn object
                thisPawn.createDestination(piecePositions, turn); //Destination array created 
                checkModePath = thisPawn.validMoveLocations;    //sets valid locations for piece
            }
            for (int i = 0; i < 8; i++)     //goes through and checks to see if a king lies in the path of the attacker
            {
                for (int j = 0; j < 8; j++)
                {
                    if (checkModePath[i, j] == true)    //if path is valid
                    {
                        if (piecePositions[i, j] == 29 || piecePositions[i, j] == 5)   //and is a king
                        {
                            Button captureButton;
                            if (piecePositions[i, j] == 5)  //get white king if it is white
                            {
                                captureButton = (Button)UIGlobal.XAMLpage.FindName($"p{5}");
                            }
                            else  //get black king if it is black
                            {
                                captureButton = (Button)UIGlobal.XAMLpage.FindName($"p{29}");
                            }
                            int kingRow = Grid.GetRow(captureButton) - 1;   //get current location of king
                            int kingCol = Grid.GetColumn(captureButton) - 1;
                            int curPieceRow = Grid.GetRow(currentPiece) - 1;    //get current location of current piece
                            int curPieceCol = Grid.GetColumn(currentPiece) - 1;
                            for (int k = 0; k < 8; k++) //go through all valid move locations and remove invalid moves under check conditions, outcome is only valid moves that defenders can go to
                            {
                                for (int l = 0; l < 8; l++)
                                {
                                    //if king is above attacker
                                    if (kingRow < curPieceRow && kingCol == curPieceCol && (k >= curPieceRow || l != curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                    }
                                    //if king is above and to the right of attacker
                                    if (kingRow < curPieceRow && kingCol > curPieceCol && (k >= curPieceRow || l <= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                    }
                                    //if king is to the left of attacker
                                    if (kingRow == curPieceRow && kingCol > curPieceCol && (k != curPieceRow || l <= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                    }
                                    //if king is down and to the right of attacker
                                    if (kingRow > curPieceRow && kingCol > curPieceCol && (k <= curPieceRow || l <= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                    }
                                    //if king is below the attacker
                                    if (kingRow > curPieceRow && kingCol == curPieceCol && (k <= curPieceRow || l != curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                    }
                                    //if king is below and to the left of attacker
                                    if (kingRow > curPieceRow && kingCol < curPieceCol && (k <= curPieceRow || l >= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                    }
                                    //if king is to the left of attacker
                                    if (kingRow == curPieceRow && kingCol < curPieceCol && (k != curPieceRow || l >= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                    }
                                    //if king is above and to the left of attacker
                                    if (kingRow < curPieceRow && kingCol < curPieceCol && (k >= curPieceRow || l >= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                    }
                                }
                            }
                            if (pieceNumber == 2 || pieceNumber == 7 || pieceNumber == 26 || pieceNumber == 31) //special conditions for the knight, no other pieces can block the knight
                            {
                                Array.Clear(checkModePath, 0, checkModePath.Length);        //clears the array of the checkmode path
                                checkModePath[kingRow, kingCol] = true; //sets the only valid location, the location of the king
                            }
                            checkModePath[curPieceRow, curPieceCol] = true; //sets a valid location so that defenders can attack the attacker
                            checkmode = true;   //sets the mode of the board into check mode.
                            CheckPopUp();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Handles showing the user the Check Text when they are there king is in check
        /// </summary>
        private void CheckPopUp()
        {
            TextBlock checkText = (TextBlock)UIGlobal.XAMLpage.FindName($"check");  //get the textblock containing the check text
            if (checkmode)  //if we are in check mode
            {
                checkText.Opacity = 1;  //show the text
            }
            else  //if we are not in check mode
            {
                checkText.Opacity = 0;  //make the text disapear
            }
        }

        private void ValidKingMovements()
        {
            Button curButton;
            if(turn == 0)
            {
                turn = 1;
                int[,] curButtonLoc = new int[8, 8];
                for (int i = 17; i < 25; i++) //go through all black pawns
                {
                    if(capturedList.Contains(i))
                    {
                        continue;
                    }
                    Array.Clear(curButtonLoc, 0, checkModePath.Length);
                    curButton = (Button)UIGlobal.XAMLpage.FindName($"p{i}");
                    curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                    Pawn thisPawn = new Pawn(curButtonLoc); //Creates new King object
                    thisPawn.createDestination(piecePositions, turn); //Destination array created 
                    if (Grid.GetRow(curButton) - 2 >= 0)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton) - 2, Grid.GetColumn(curButton) - 1] = false;
                    }
                    if (Grid.GetRow(curButton) - 3 >= 0)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton) - 3, Grid.GetColumn(curButton) - 1] = false;
                    }
                    if (Grid.GetColumn(curButton) - 2 >= 0 && Grid.GetRow(curButton) - 2 >= 0)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton) - 2, Grid.GetColumn(curButton) - 2] = true;
                    }
                    if (Grid.GetColumn(curButton) < 8 && Grid.GetRow(curButton) - 2 >= 0)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton) - 2, Grid.GetColumn(curButton)] = true;

                    }
                    validKingMovements = OrArrays(validKingMovements, thisPawn.validMoveLocations);
                }
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{25}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Rook thisRook = new Rook(curButtonLoc); //Creates new King object
                thisRook.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisRook.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{32}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisRook = new Rook(curButtonLoc); //Creates new King object
                thisRook.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisRook.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{26}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Knight thisKnight = new Knight(curButtonLoc); //Creates new King object
                thisKnight.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKnight.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{31}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisKnight = new Knight(curButtonLoc); //Creates new King object
                thisKnight.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKnight.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{27}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Bishop thisBishop = new Bishop(curButtonLoc); //Creates new King object
                thisBishop.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisBishop.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{30}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisBishop = new Bishop(curButtonLoc); //Creates new King object
                thisBishop.createDestination(piecePositions, turn); //Destination array created    
                validKingMovements = OrArrays(validKingMovements, thisBishop.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{28}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Queen thisQueen = new Queen(curButtonLoc); //Creates new King object
                thisQueen.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisQueen.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{29}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                King thisKing = new King(curButtonLoc); //Creates new King object
                thisKing.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKing.validMoveLocations);

                turn = 0;
            }
            else
            {
                turn = 0;

                int[,] curButtonLoc = new int[8, 8];
                for (int i = 9; i < 17; i++) //go through all white pawns
                {
                    if (capturedList.Contains(i))
                    {
                        continue;
                    }
                    Array.Clear(curButtonLoc, 0, checkModePath.Length);
                    curButton = (Button)UIGlobal.XAMLpage.FindName($"p{i}");
                    curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                    Pawn thisPawn = new Pawn(curButtonLoc); //Creates new King object
                    thisPawn.createDestination(piecePositions, turn); //Destination array created
                    if (Grid.GetRow(curButton) < 8)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton), Grid.GetColumn(curButton) - 1] = false;
                    }
                    if (Grid.GetRow(curButton) + 1 < 8)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton) + 1, Grid.GetColumn(curButton) - 1] = false;
                    }
                    if (Grid.GetColumn(curButton) < 8 && Grid.GetRow(curButton) < 8)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton), Grid.GetColumn(curButton)] = true;
                    }
                    if (Grid.GetColumn(curButton) - 2 >= 0 && Grid.GetRow(curButton) < 8)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton), Grid.GetColumn(curButton) - 2] = true;
                    }
                    validKingMovements = OrArrays(validKingMovements, thisPawn.validMoveLocations);
                }
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{1}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Rook thisRook = new Rook(curButtonLoc); //Creates new King object
                thisRook.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisRook.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{8}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisRook = new Rook(curButtonLoc); //Creates new King object
                thisRook.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisRook.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{2}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Knight thisKnight = new Knight(curButtonLoc); //Creates new King object
                thisKnight.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKnight.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{7}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisKnight = new Knight(curButtonLoc); //Creates new King object
                thisKnight.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKnight.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{3}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Bishop thisBishop = new Bishop(curButtonLoc); //Creates new King object
                thisBishop.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisBishop.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{6}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisBishop = new Bishop(curButtonLoc); //Creates new King object
                thisBishop.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisBishop.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{4}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Queen thisQueen = new Queen(curButtonLoc); //Creates new King object
                thisQueen.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisQueen.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{5}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                King thisKing = new King(curButtonLoc); //Creates new King object
                thisKing.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKing.validMoveLocations);

                turn = 1;
            }
        }

        private bool[,] OrArrays(bool[,] array1, bool[,] array2)
        {
            bool[,] returnarray = new bool[8, 8];
            for(int i = 0; i < 8; i++)
            {
                for(int k = 0; k < 8; k++)
                {
                    if (array1[i,k] == true || array2[i,k] == true)
                    {
                        returnarray[i, k] = true;
                    }
                }
            }
            return returnarray;
        }

    }
}