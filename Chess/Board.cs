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

        private int[] checkAttackerLocation = new int[2];

        private int[] invalidSingleKingMovement = new int[2];

        private int buttonCount = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        public Board()
        {
            turn = 0;   //turn starts out with white piece
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
                Grid.SetRow(captureButton, capturedPieceWhite[0]);  //sets the row and column in capture grid
                Grid.SetColumn(captureButton, capturedPieceWhite[1]);
                captureButton.IsEnabled = false;    //disables the button
                captureButton.Opacity = 1;
                UIGlobal.getGrid().Children.Remove(captureButton);  //removes from main grid
                UIGlobal.getPlayer1Grid().Children.Add(captureButton);  //and adds to capture grid
                if (capturedPieceWhite[1] == 2) //increments grid location 
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
                Grid.SetRow(captureButton, capturedPieceBlack[0]);  //sets the row and column in capture grid
                Grid.SetColumn(captureButton, capturedPieceBlack[1]);   
                captureButton.IsEnabled = false;    //disables the button
                captureButton.Opacity = 1;
                UIGlobal.getGrid().Children.Remove(captureButton);  //removes from main grid
                UIGlobal.getPlayer2Grid().Children.Add(captureButton);  //and adds to capture grid
                if (capturedPieceBlack[1] == 2) //increments grid location
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
            int buttonCount = 0;
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
            bool[,] validKingMovements = new bool[8, 8];
            validKingMovements = ValidKingMovements();
            if (checkmode)
            {
                if (pieceNumber == 5 || pieceNumber == 29) //Checks if piece is King
                {
                    King thisKing = new King(selectedPiece); //Creates new King object
                    thisKing.createDestination(piecePositions, turn); //Destination array created 

                    checkModePath[checkAttackerLocation[0], checkAttackerLocation[1]] = (true ^ checkModePath[checkAttackerLocation[0], checkAttackerLocation[1]]); //toggles attacker cell so that king can attack it if it is right by the king
                    checkModePath[invalidSingleKingMovement[0], invalidSingleKingMovement[1]] = (true ^ checkModePath[invalidSingleKingMovement[0], invalidSingleKingMovement[1]]);
                    for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for King Movement
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (thisKing.validMoveLocations[i, j] == true && checkModePath[i, j] == false  && validKingMovements[i, j] == false)  //can only move to spots that wouldn't keep it in check
                            {
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
                            }
                        }
                    }
                    checkModePath[checkAttackerLocation[0], checkAttackerLocation[1]] = (true ^ checkModePath[checkAttackerLocation[0], checkAttackerLocation[1]]); //resets to original value
                    checkModePath[invalidSingleKingMovement[0], invalidSingleKingMovement[1]] = (true ^ checkModePath[invalidSingleKingMovement[0], invalidSingleKingMovement[1]]);
                }
                else if (pieceNumber == 1 || pieceNumber == 8 || pieceNumber == 25 || pieceNumber == 32) //Checks if piece is Rook
                {
                    Rook thisRook = new Rook(selectedPiece); //Creates new rook object
                    thisRook.CreateDestination(piecePositions, turn); //Destination array created 
                    for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for Rook Movement
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (thisRook.validMoveLocations[i, j] == true && checkModePath[i, j] == true)   //can move to only spots defending the king
                            {
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
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
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
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
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
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
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
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
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
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
                    thisRook.CreateDestination(piecePositions, turn); //Destination array created 
                    for (int i = 0; i < 8; i++) //Nested for loop to create available buttons for Rook Movement
                    {
                        for (int j = 0; j < 8; j++)
                        {
                            if (thisRook.validMoveLocations[i, j] == true)
                            {
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
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
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
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
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
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
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
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
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
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
                                if (!WillNextMoveCauseCheck(i, j))
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
                                    buttonCount++;
                                }
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
                thisRook.CreateDestination(piecePositions, turn); //Destination array created 
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
                            checkAttackerLocation[0] = curPieceRow;     //for use in king attacking attacker on defense
                            checkAttackerLocation[1] = curPieceCol;

                            int[] pawnarray = new int[16] { 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24 };

                            for (int k = 0; k < 8; k++) //go through all valid move locations and remove invalid moves under check conditions, outcome is only valid moves that defenders can go to
                            {
                                for (int l = 0; l < 8; l++)
                                {
                                    //if king is above attacker
                                    if (kingRow < curPieceRow && kingCol == curPieceCol && (k >= curPieceRow || l != curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                        if(kingRow - 1 >= 0 && !(pawnarray.Contains(pieceNumber)))  //solves the problem of extra invalid position behind king on check except for pawns causing check
                                        {
                                            invalidSingleKingMovement[0] = kingRow - 1; //stores this invalid move location
                                            invalidSingleKingMovement[1] = kingCol;
                                        }
                                    }
                                    //if king is above and to the right of attacker
                                    if (kingRow < curPieceRow && kingCol > curPieceCol && (k >= curPieceRow || l <= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                        if (kingRow - 1 >= 0  && kingCol + 1 < 8 && !(pawnarray.Contains(pieceNumber))) //solves the problem of extra invalid position behind king on check except for pawns causing check
                                        {
                                            invalidSingleKingMovement[0] = kingRow - 1; //stores this invalid move location
                                            invalidSingleKingMovement[1] = kingCol + 1;
                                        }
                                    }
                                    //if king is to the right of attacker
                                    if (kingRow == curPieceRow && kingCol > curPieceCol && (k != curPieceRow || l <= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                        if (kingCol + 1 < 8 && !(pawnarray.Contains(pieceNumber)))  //solves the problem of extra invalid position behind king on check except for pawns causing check
                                        {
                                            invalidSingleKingMovement[0] = kingRow; //stores this invalid move location
                                            invalidSingleKingMovement[1] = kingCol + 1;
                                        }
                                    }
                                    //if king is down and to the right of attacker
                                    if (kingRow > curPieceRow && kingCol > curPieceCol && (k <= curPieceRow || l <= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                        if (kingRow + 1 < 8 && kingCol + 1 < 8 && !(pawnarray.Contains(pieceNumber)))   //solves the problem of extra invalid position behind king on check except for pawns causing check
                                        {
                                            invalidSingleKingMovement[0] = kingRow + 1; //stores this invalid move location
                                            invalidSingleKingMovement[1] = kingCol + 1;
                                        }
                                    }
                                    //if king is below the attacker
                                    if (kingRow > curPieceRow && kingCol == curPieceCol && (k <= curPieceRow || l != curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                        if (kingRow + 1 < 8 && !(pawnarray.Contains(pieceNumber)))  //solves the problem of extra invalid position behind king on check except for pawns causing check
                                        {
                                            invalidSingleKingMovement[0] = kingRow + 1; //stores this invalid move location
                                            invalidSingleKingMovement[1] = kingCol;
                                        }
                                    }
                                    //if king is below and to the left of attacker
                                    if (kingRow > curPieceRow && kingCol < curPieceCol && (k <= curPieceRow || l >= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                        if (kingRow + 1 < 8 && kingCol - 1 >= 0 && !(pawnarray.Contains(pieceNumber)))  //solves the problem of extra invalid position behind king on check except for pawns causing check
                                        {
                                            invalidSingleKingMovement[0] = kingRow + 1; //stores this invalid move location
                                            invalidSingleKingMovement[1] = kingCol - 1;
                                        }
                                    }
                                    //if king is to the left of attacker
                                    if (kingRow == curPieceRow && kingCol < curPieceCol && (k != curPieceRow || l >= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                        if (kingCol - 1 >= 0 && !(pawnarray.Contains(pieceNumber))) //solves the problem of extra invalid position behind king on check except for pawns causing check
                                        {
                                            invalidSingleKingMovement[0] = kingRow; //stores this invalid move location
                                            invalidSingleKingMovement[1] = kingCol - 1;
                                        }
                                    }
                                    //if king is above and to the left of attacker
                                    if (kingRow < curPieceRow && kingCol < curPieceCol && (k >= curPieceRow || l >= curPieceCol) && checkModePath[k, l] == true)
                                    {
                                        checkModePath[k, l] = false;
                                        if (kingRow - 1 >= 0 && kingCol - 1 >= 0 && !(pawnarray.Contains(pieceNumber)))   //solves the problem of extra invalid position behind king on check except for pawns causing check
                                        {
                                            invalidSingleKingMovement[0] = kingRow - 1; //stores this invalid move location
                                            invalidSingleKingMovement[1] = kingCol - 1;
                                        }
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

        /// <summary>
        /// goes through and gets all the valid movments of the opposing sides pieces. 
        /// Is used to see where the king can go to make sure the kings movement does not
        /// result in a check
        /// </summary>
        private bool[,] ValidKingMovements()
        {
            bool[,] validKingMovements = new bool[8, 8];
            Button curButton;
            if(turn == 0)   //whites turn
            {
                turn = 1;
                int[,] curButtonLoc = new int[8, 8];
                for (int i = 17; i < 25; i++) //go through all black pawns
                {
                    if(capturedList.Contains(i))    //makes sure the piece isn't captured
                    {
                        continue;
                    }
                    Array.Clear(curButtonLoc, 0, checkModePath.Length);     //clears the current locations from the last piece
                    curButton = (Button)UIGlobal.XAMLpage.FindName($"p{i}");    //gets the pawn
                    curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                    Pawn thisPawn = new Pawn(curButtonLoc); //Creates new Pawn object
                    thisPawn.createDestination(piecePositions, turn); //Destination array created 
                    //gets rid of valid locations for one place in front of pawn
                    if (Grid.GetRow(curButton) - 2 >= 0)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton) - 2, Grid.GetColumn(curButton) - 1] = false;
                    }
                    //gets rid of valid locations for two places in front of pawn
                    if (Grid.GetRow(curButton) - 3 >= 0)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton) - 3, Grid.GetColumn(curButton) - 1] = false;
                    }
                    //sets valid location for in front of and to the right and left
                    if (Grid.GetColumn(curButton) - 2 >= 0 && Grid.GetRow(curButton) - 2 >= 0)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton) - 2, Grid.GetColumn(curButton) - 2] = true;
                    }
                    if (Grid.GetColumn(curButton) < 8 && Grid.GetRow(curButton) - 2 >= 0)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton) - 2, Grid.GetColumn(curButton)] = true;

                    }
                    //ors the valid moves with the existing moves
                    validKingMovements = OrArrays(validKingMovements, thisPawn.validMoveLocations);
                }

                //handles getting rooks valid locations and ors it with valid locations
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{25}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Rook thisRook = new Rook(curButtonLoc); //Creates new rook object
                thisRook.CreateDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisRook.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{32}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisRook = new Rook(curButtonLoc);
                thisRook.CreateDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisRook.validMoveLocations);

                //handles getting knights valid locations and ors it with valid locations
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{26}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Knight thisKnight = new Knight(curButtonLoc); //Creates new Knight object
                thisKnight.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKnight.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{31}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisKnight = new Knight(curButtonLoc); //Creates new King object
                thisKnight.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKnight.validMoveLocations);

                //handles getting bishop valid locations and ors it with valid locations
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{27}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Bishop thisBishop = new Bishop(curButtonLoc); //Creates new bishop object
                thisBishop.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisBishop.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{30}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisBishop = new Bishop(curButtonLoc); //Creates new King object
                thisBishop.createDestination(piecePositions, turn); //Destination array created    
                validKingMovements = OrArrays(validKingMovements, thisBishop.validMoveLocations);

                //handles getting queen valid locations and ors it with valid locations
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{28}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Queen thisQueen = new Queen(curButtonLoc); //Creates new King object
                thisQueen.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisQueen.validMoveLocations);

                //handles getting king valid locations and ors it with valid locations
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{29}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                King thisKing = new King(curButtonLoc); //Creates new King object
                thisKing.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKing.validMoveLocations);

                //sets turn back to white
                turn = 0;
            }
            else  //if the turn is blacks
            {
                turn = 0;

                int[,] curButtonLoc = new int[8, 8];    //current location of button
                for (int i = 9; i < 17; i++) //go through all white pawns
                {
                    if (capturedList.Contains(i))   //checks if pawn is already captured
                    {
                        continue;
                    }
                    Array.Clear(curButtonLoc, 0, checkModePath.Length); //clears the location of the previous button
                    curButton = (Button)UIGlobal.XAMLpage.FindName($"p{i}");    //finds pawn in xaml
                    curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                    Pawn thisPawn = new Pawn(curButtonLoc); //Creates new pawn object
                    thisPawn.createDestination(piecePositions, turn); //Destination array created
                    //gets rid of valid locations for one place in front of pawn
                    if (Grid.GetRow(curButton) < 8)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton), Grid.GetColumn(curButton) - 1] = false;
                    }
                    //gets rid of valid locations for two places in front of pawn
                    if (Grid.GetRow(curButton) + 1 < 8)
                    {
                        thisPawn.validMoveLocations[Grid.GetRow(curButton) + 1, Grid.GetColumn(curButton) - 1] = false;
                    }
                    //sets valid location for in front of and to the right and left
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

                //handles getting Rook valid locations and ors it with valid locations
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{1}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Rook thisRook = new Rook(curButtonLoc); //Creates new Rook object
                thisRook.CreateDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisRook.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{8}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisRook = new Rook(curButtonLoc); //Creates new King object
                thisRook.CreateDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisRook.validMoveLocations);

                //handles getting Knight valid locations and ors it with valid locations
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{2}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Knight thisKnight = new Knight(curButtonLoc); //Creates new Knight object
                thisKnight.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKnight.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{7}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisKnight = new Knight(curButtonLoc); //Creates new King object
                thisKnight.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKnight.validMoveLocations);

                //handles getting Bishop valid locations and ors it with valid locations
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{3}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Bishop thisBishop = new Bishop(curButtonLoc); //Creates new Bishop object
                thisBishop.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisBishop.validMoveLocations);

                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{6}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                thisBishop = new Bishop(curButtonLoc); //Creates new King object
                thisBishop.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisBishop.validMoveLocations);

                //handles getting Queen valid locations and ors it with valid locations
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{4}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                Queen thisQueen = new Queen(curButtonLoc); //Creates new Queen object
                thisQueen.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisQueen.validMoveLocations);

                //handles getting king valid locations and ors it with valid locations
                curButton = (Button)UIGlobal.XAMLpage.FindName($"p{5}");
                Array.Clear(curButtonLoc, 0, checkModePath.Length);
                curButtonLoc[Grid.GetRow(curButton) - 1, Grid.GetColumn(curButton) - 1] = 1;//get the location of the current button
                King thisKing = new King(curButtonLoc); //Creates new King object
                thisKing.createDestination(piecePositions, turn); //Destination array created 
                validKingMovements = OrArrays(validKingMovements, thisKing.validMoveLocations);

                //set turn back to black
                turn = 1;
            }
            return validKingMovements;
        }

        /// <summary>
        /// takes two inputs arrays and ors them together returning a new array 
        /// with the results
        /// </summary>
        /// <param name="array1"></param>
        /// <param name="array2"></param>
        /// <returns></returns>
        private bool[,] OrArrays(bool[,] array1, bool[,] array2)
        {
            //returned or array
            bool[,] returnarray = new bool[8, 8];
            //goes through and ors the two arrays together and puts the results in the returnarray
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

        private bool WillNextMoveCauseCheck(int newRow, int newCol)
        {
            int prevRow = 0;
            int prevCol = 0;
            int playersPieceNumber = 0;
            for(int j = 0; j < 8; j++)
            {
                for(int k = 0; k < 8; k++)
                {
                    if (selectedPiece[j, k] == 1)
                    {
                        prevRow = j;
                        prevCol = k;
                        playersPieceNumber = piecePositions[j, k];
                    }
                }
            }
            int attackedPieceNumber = piecePositions[newRow, newCol];

            Button prevButton = (Button)UIGlobal.XAMLpage.FindName($"p{playersPieceNumber}");
            Button newButton = (Button)UIGlobal.XAMLpage.FindName($"p{attackedPieceNumber}");

            bool[,] validKingMovements = new bool[8, 8];

            int prevPieceNumber = piecePositions[newRow, newCol];

            if (newButton != null)
            {
                Grid.SetRow(newButton, 1);
                Grid.SetColumn(newButton, 1);
                UIGlobal.getGrid().Children.Remove(newButton);  //removes from main grid
                UIGlobal.getPlayer1Grid().Children.Add(newButton);  //and adds to capture grid

                Grid.SetRow(prevButton, newRow + 1);
                Grid.SetColumn(prevButton, newCol + 1);

                piecePositions[prevRow, prevCol] = 0;
                piecePositions[newRow, newCol] = playersPieceNumber;

                validKingMovements = ValidKingMovements();

                piecePositions[newRow, newCol] = prevPieceNumber;
                piecePositions[prevRow, prevCol] = playersPieceNumber;

                Grid.SetRow(prevButton, prevRow + 1);
                Grid.SetColumn(prevButton, prevCol + 1);

                Grid.SetRow(newButton, newRow + 1);  //sets the row and column in capture grid
                Grid.SetColumn(newButton, newCol + 1);
                UIGlobal.getPlayer1Grid().Children.Remove(newButton);  //and adds to capture grid
                UIGlobal.getGrid().Children.Add(newButton);  //removes from main grid
            }
            else
            {
                Grid.SetRow(prevButton, newRow + 1);
                Grid.SetColumn(prevButton, newCol + 1);

                piecePositions[prevRow, prevCol] = 0;
                piecePositions[newRow, newCol] = playersPieceNumber;

                validKingMovements = ValidKingMovements();

                piecePositions[newRow, newCol] = prevPieceNumber;
                piecePositions[prevRow, prevCol] = playersPieceNumber;

                Grid.SetRow(prevButton, prevRow + 1);
                Grid.SetColumn(prevButton, prevCol + 1);
            }
            if(playersPieceNumber == 5 || playersPieceNumber == 29)
            {
                piecePositions[prevRow, prevCol] = 0;
                piecePositions[newRow, newCol] = playersPieceNumber;
            }
            for (int l = 0; l < 8; l++)
            {
                for (int p = 0; p < 8; p++)
                {
                    if (validKingMovements[l, p] == true)
                    {
                        if (turn == 0 && piecePositions[l, p] == 5)
                        {
                            piecePositions[newRow, newCol] = prevPieceNumber;
                            piecePositions[prevRow, prevCol] = playersPieceNumber;
                            return true;
                        }
                        if (turn == 1 && piecePositions[l, p] == 29)
                        {
                            piecePositions[newRow, newCol] = prevPieceNumber;
                            piecePositions[prevRow, prevCol] = playersPieceNumber;
                            return true;
                        }
                    }
                }
            }
            piecePositions[newRow, newCol] = prevPieceNumber;
            piecePositions[prevRow, prevCol] = playersPieceNumber;
            return false;
        }

        private void CheckMateStaleMateCheck()
        {
            TextBlock checkmateText = (TextBlock)UIGlobal.XAMLpage.FindName($"checkmate");  //get the textblock containing the check text
            if (buttonCount == 0)  //if we are in checkmate
            {
                checkmateText.Opacity = 1;  //show the text
            }
        }

        public override bool Equals(object obj)
        {
            var board = obj as Board;
            return board != null &&
                   EqualityComparer<int[,]>.Default.Equals(piecePositions, board.piecePositions) &&
                   EqualityComparer<int[,]>.Default.Equals(selectedPiece, board.selectedPiece) &&
                   EqualityComparer<List<Button>>.Default.Equals(createdButtons, board.createdButtons) &&
                   EqualityComparer<Button>.Default.Equals(currentPiece, board.currentPiece) &&
                   finishedMoveFlag == board.finishedMoveFlag &&
                   turn == board.turn &&
                   EqualityComparer<int[]>.Default.Equals(capturedPieceWhite, board.capturedPieceWhite) &&
                   EqualityComparer<int[]>.Default.Equals(capturedPieceBlack, board.capturedPieceBlack) &&
                   EqualityComparer<List<int>>.Default.Equals(capturedList, board.capturedList) &&
                   checkmode == board.checkmode &&
                   EqualityComparer<bool[,]>.Default.Equals(checkModePath, board.checkModePath);
        }

        public override int GetHashCode()
        {
            var hashCode = -838244041;
            hashCode = hashCode * -1521134295 + EqualityComparer<int[,]>.Default.GetHashCode(piecePositions);
            hashCode = hashCode * -1521134295 + EqualityComparer<int[,]>.Default.GetHashCode(selectedPiece);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<Button>>.Default.GetHashCode(createdButtons);
            hashCode = hashCode * -1521134295 + EqualityComparer<Button>.Default.GetHashCode(currentPiece);
            hashCode = hashCode * -1521134295 + finishedMoveFlag.GetHashCode();
            hashCode = hashCode * -1521134295 + turn.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<int[]>.Default.GetHashCode(capturedPieceWhite);
            hashCode = hashCode * -1521134295 + EqualityComparer<int[]>.Default.GetHashCode(capturedPieceBlack);
            hashCode = hashCode * -1521134295 + EqualityComparer<List<int>>.Default.GetHashCode(capturedList);
            hashCode = hashCode * -1521134295 + checkmode.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<bool[,]>.Default.GetHashCode(checkModePath);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}