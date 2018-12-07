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
    public class Piece
    {
        protected int[] Location = new int[2];  //first indicy give us the the row number, second indicy gives us column number
        public bool[,] validMoveLocations = new bool[8, 8]; //2-D integer array denotes available locations for rook to move, accessed by board to create buttons for movement
        /// <summary>
        /// The constructor takes an integer array of the location of the piece. It then passes the value into the pieceLocation field. 
        /// </summary>
        /// <param name="location"></param>
        public Piece(int[,] loc)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (loc[i, j] == 1)
                    {
                        Location[0] = i; //row is stored in the first index of Location
                        Location[1] = j; //col is stored in the second index of Location
                    }
                }
            }
        }
    }
    public class Rook : Piece
    {
        public Rook(int[,] loc) : base(loc)
        {

        }

        public void createDestination(int[,] pieces, int turn)
        {
            int i = 0;
            int j = 0;
            //sets the path for right movement
            for (i = Location[1] + 1; i < 8; i++)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[Location[0], i] > 16) //if it is a black piece
                    {
                        validMoveLocations[Location[0], i] = true;
                        break;
                    }
                    else if (pieces[Location[0], i] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[Location[0], i] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[Location[0], i] < 17 && pieces[Location[0], i] > 0) //if it is a white piece
                    {
                        validMoveLocations[Location[0], i] = true;
                        break;
                    }
                    else if (pieces[Location[0], i] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[Location[0], i] = true;
                    }
                }
            }
            //sets the path for left movement
            for (i = Location[1] - 1; i >= 0; i--) //for loop to set the row of the piece location to true for the move locations
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[Location[0], i] > 16) //if it is a black piece
                    {
                        validMoveLocations[Location[0], i] = true;
                        break;
                    }
                    else if (pieces[Location[0], i] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[Location[0], i] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[Location[0], i] < 17 && pieces[Location[0], i] > 0) //if it is a white piece
                    {
                        validMoveLocations[Location[0], i] = true;
                        break;
                    }
                    else if (pieces[Location[0], i] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[Location[0], i] = true;
                    }
                }
            }
            //sets the path for down movement
            for (j = Location[0] + 1; j < 8; j++) //for loop to set the column of the piece location to true for the move locations
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[j, Location[1]] > 16) //if it is a black piece
                    {
                        validMoveLocations[j, Location[1]] = true;
                        break;
                    }
                    else if (pieces[j, Location[1]] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[j, Location[1]] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[j, Location[1]] < 17 && pieces[j, Location[1]] > 0) //if it is a white piece
                    {
                        validMoveLocations[j, Location[1]] = true;
                        break;
                    }
                    else if (pieces[j, Location[1]] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[j, Location[1]] = true;
                    }
                }
            }
            //sets the path for up movement
            for (j = Location[0] - 1; j >= 0; j--) //for loop to set the column of the piece location to true for the move locations
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[j, Location[1]] > 16) //if it is a black piece
                    {
                        validMoveLocations[j, Location[1]] = true;
                        break;
                    }
                    else if (pieces[j, Location[1]] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[j, Location[1]] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[j, Location[1]] < 17 && pieces[j, Location[1]] > 0) //if it is a white piece
                    {
                        validMoveLocations[j, Location[1]] = true;
                        break;
                    }
                    else if (pieces[j, Location[1]] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[j, Location[1]] = true;
                    }
                }
            }
        }

    }
    public class Bishop : Piece
    {
        public Bishop(int[,] loc) : base(loc)
        {

        }

        /// <summary>
        /// Function updates the valid move locations array with a true or false for each index. True means the
        /// object can move to that place and false means it cannot. 
        /// </summary>
        public void createDestination(int[,] pieces, int turn)
        {
            //set the path for down and right
            int i = Location[0] + 1;
            int j = Location[1] + 1;
            while (i < 8 && j < 8) //diagonal for down and right
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i, j] > 16) //if it is a black piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i, j] < 17 && pieces[i, j] > 0) //if it is a white piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                i++;
                j++;
            }

            //set the path for up and left
            i = Location[0] - 1;
            j = Location[1] - 1;
            while (i >= 0 && j >= 0) //diagonal for up and left
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i, j] > 16) //if it is a black piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i, j] < 17 && pieces[i, j] > 0) //if it is a white piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                i--;
                j--;
            }

            //set the path for down and left
            i = Location[0] + 1;
            j = Location[1] - 1;
            while (i < 8 && j >= 0) //diagonal for down and left
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i, j] > 16) //if it is a black piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i, j] < 17 && pieces[i, j] > 0) //if it is a white piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                i++;
                j--;
            }

            //set the path for up and right
            i = Location[0] - 1;
            j = Location[1] + 1;
            while (i >= 0 && j < 8) //diagonal for up and right
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i, j] > 16) //if it is a black piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i, j] < 17 && pieces[i, j] > 0) //if it is a white piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                i--;
                j++;
            }
        }
    }
    public class Queen : Piece
    {
        public Queen(int[,] loc) : base(loc)
        {

        }

        public void createDestination(int[,] pieces, int turn)
        {
            int i = 0;
            int j = 0;
            //sets the path for right movement
            for (i = Location[1] + 1; i < 8; i++)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[Location[0], i] > 16) //if it is a black piece
                    {
                        validMoveLocations[Location[0], i] = true;
                        break;
                    }
                    else if (pieces[Location[0], i] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[Location[0], i] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[Location[0], i] < 17 && pieces[Location[0], i] > 0) //if it is a white piece
                    {
                        validMoveLocations[Location[0], i] = true;
                        break;
                    }
                    else if (pieces[Location[0], i] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[Location[0], i] = true;
                    }
                }
            }
            //sets the path for left movement
            for (i = Location[1] - 1; i >= 0; i--) //for loop to set the row of the piece location to true for the move locations
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[Location[0], i] > 16) //if it is a black piece
                    {
                        validMoveLocations[Location[0], i] = true;
                        break;
                    }
                    else if (pieces[Location[0], i] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[Location[0], i] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[Location[0], i] < 17 && pieces[Location[0], i] > 0) //if it is a white piece
                    {
                        validMoveLocations[Location[0], i] = true;
                        break;
                    }
                    else if (pieces[Location[0], i] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[Location[0], i] = true;
                    }
                }
            }
            //sets the path for down movement
            for (j = Location[0] + 1; j < 8; j++) //for loop to set the column of the piece location to true for the move locations
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[j, Location[1]] > 16) //if it is a black piece
                    {
                        validMoveLocations[j, Location[1]] = true;
                        break;
                    }
                    else if (pieces[j, Location[1]] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[j, Location[1]] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[j, Location[1]] < 17 && pieces[j, Location[1]] > 0) //if it is a white piece
                    {
                        validMoveLocations[j, Location[1]] = true;
                        break;
                    }
                    else if (pieces[j, Location[1]] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[j, Location[1]] = true;
                    }
                }
            }
            //sets the path for up movement
            for (j = Location[0] - 1; j >= 0; j--) //for loop to set the column of the piece location to true for the move locations
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[j, Location[1]] > 16) //if it is a black piece
                    {
                        validMoveLocations[j, Location[1]] = true;
                        break;
                    }
                    else if (pieces[j, Location[1]] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[j, Location[1]] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[j, Location[1]] < 17 && pieces[j, Location[1]] > 0) //if it is a white piece
                    {
                        validMoveLocations[j, Location[1]] = true;
                        break;
                    }
                    else if (pieces[j, Location[1]] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[j, Location[1]] = true;
                    }
                }
            }

            //set the path for down and right
            i = Location[0] + 1;
            j = Location[1] + 1;
            while (i < 8 && j < 8) //diagonal for down and right
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i, j] > 16) //if it is a black piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i, j] < 17 && pieces[i, j] > 0) //if it is a white piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                i++;
                j++;
            }

            //set the path for up and left
            i = Location[0] - 1;
            j = Location[1] - 1;
            while (i >= 0 && j >= 0) //diagonal for up and left
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i, j] > 16) //if it is a black piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i, j] < 17 && pieces[i, j] > 0) //if it is a white piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                i--;
                j--;
            }

            //set the path for down and left
            i = Location[0] + 1;
            j = Location[1] - 1;
            while (i < 8 && j >= 0) //diagonal for down and left
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i, j] > 16) //if it is a black piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i, j] < 17 && pieces[i, j] > 0) //if it is a white piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                i++;
                j--;
            }

            //set the path for up and right
            i = Location[0] - 1;
            j = Location[1] + 1;
            while (i >= 0 && j < 8) //diagonal for up and right
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i, j] > 16) //if it is a black piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 0)   //if it is a white piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i, j] < 17 && pieces[i, j] > 0) //if it is a white piece
                    {
                        validMoveLocations[i, j] = true;
                        break;
                    }
                    else if (pieces[i, j] > 16)   //if it is a black piece
                    {
                        break;
                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j] = true;
                    }
                }
                i--;
                j++;
            }


        }
    }
    public class Knight : Piece
    {
        public Knight(int[,] loc) : base(loc)
        {

        }

        /// <summary>
        /// Function updates the valid move locations array with a true or false for each index. True means the
        /// object can move to that place and false means it cannot. 
        /// </summary>
        public void createDestination(int[,] pieces, int turn)
        {
            int i = Location[0];
            int j = Location[1];

            //down 2 right 1
            if (i + 2 < 8 && j + 1 < 8)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i + 2, j + 1] > 16) //if it is a black piece
                    {
                        validMoveLocations[i + 2, j + 1] = true;
                    }
                    else if (pieces[i + 2, j + 1] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 2, j + 1] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i + 2, j + 1] < 17 && pieces[i + 2, j + 1] > 0) //if it is a white piece
                    {
                        validMoveLocations[i + 2, j + 1] = true;
                    }
                    else if (pieces[i + 2, j + 1] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 2, j + 1] = true;
                    }
                }
            }

            //down 1 right 2  
            if (i + 1 < 8 && j + 2 < 8)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i + 1, j + 2] > 16) //if it is a black piece
                    {
                        validMoveLocations[i + 1, j + 2] = true;
                    }
                    else if (pieces[i + 1, j + 2] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 1, j + 2] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i + 1, j + 2] < 17 && pieces[i + 1, j + 2] > 0) //if it is a white piece
                    {
                        validMoveLocations[i + 1, j + 2] = true;
                    }
                    else if (pieces[i + 1, j + 2] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 1, j + 2] = true;
                    }
                }
            }

            //down 2 left 1
            if (i + 2 < 8 && j - 1 >= 0)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i + 2, j - 1] > 16) //if it is a black piece
                    {
                        validMoveLocations[i + 2, j - 1] = true;
                    }
                    else if (pieces[i + 2, j - 1] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 2, j - 1] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i + 2, j - 1] < 17 && pieces[i + 2, j - 1] > 0) //if it is a white piece
                    {
                        validMoveLocations[i + 2, j - 1] = true;
                    }
                    else if (pieces[i + 2, j - 1] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 2, j - 1] = true;
                    }
                }
            }

            //down 1 left 2
            if (i + 1 < 8 && j - 2 >= 0)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i + 1, j - 2] > 16) //if it is a black piece
                    {
                        validMoveLocations[i + 1, j - 2] = true;
                    }
                    else if (pieces[i + 1, j - 2] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 1, j - 2] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i + 1, j - 2] < 17 && pieces[i + 1, j - 2] > 0) //if it is a white piece
                    {
                        validMoveLocations[i + 1, j - 2] = true;
                    }
                    else if (pieces[i + 1, j - 2] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 1, j - 2] = true;
                    }
                }
            }

            //up 2  right 1
            if (i - 2 >= 0 && j + 1 < 8)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i - 2, j + 1] > 16) //if it is a black piece
                    {
                        validMoveLocations[i - 2, j + 1] = true;
                    }
                    else if (pieces[i - 2, j + 1] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 2, j + 1] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i - 2, j + 1] < 17 && pieces[i - 2, j + 1] > 0) //if it is a white piece
                    {
                        validMoveLocations[i - 2, j + 1] = true;
                    }
                    else if (pieces[i - 2, j + 1] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 2, j + 1] = true;
                    }
                }
            }

            //up 1 right 2  
            if (i - 1 >= 0 && j + 2 < 8)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i - 1, j + 2] > 16) //if it is a black piece
                    {
                        validMoveLocations[i - 1, j + 2] = true;
                    }
                    else if (pieces[i - 1, j + 2] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 1, j + 2] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i - 1, j + 2] < 17 && pieces[i - 1, j + 2] > 0) //if it is a white piece
                    {
                        validMoveLocations[i - 1, j + 2] = true;
                    }
                    else if (pieces[i - 1, j + 2] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 1, j + 2] = true;
                    }
                }
            }

            //up 2 left 1
            if (i - 2 >= 0 && j - 1 >= 0)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i - 2, j - 1] > 16) //if it is a black piece
                    {
                        validMoveLocations[i - 2, j - 1] = true;
                    }
                    else if (pieces[i - 2, j - 1] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 2, j - 1] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i - 2, j - 1] < 17 && pieces[i - 2, j - 1] > 0) //if it is a white piece
                    {
                        validMoveLocations[i - 2, j - 1] = true;
                    }
                    else if (pieces[i - 2, j - 1] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 2, j - 1] = true;
                    }
                }
            }

            //up 1 left 2
            if (i - 1 >= 0 && j - 2 >= 0)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i - 1, j - 2] > 16) //if it is a black piece
                    {
                        validMoveLocations[i - 1, j - 2] = true;
                    }
                    else if (pieces[i - 1, j - 2] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 1, j - 2] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i - 1, j - 2] < 17 && pieces[i - 1, j - 2] > 0) //if it is a white piece
                    {
                        validMoveLocations[i - 1, j - 2] = true;
                    }
                    else if (pieces[i - 1, j - 2] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 1, j - 2] = true;
                    }
                }
            }
        }
    }
    public class King : Piece
    {
        public King(int[,] loc) : base(loc)
        {

        }

        /// <summary>
        /// Function updates the valid move locations array with a true or false for each index. True means the
        /// object can move to that place and false means it cannot. 
        /// </summary>
        public void createDestination(int[,] pieces, int turn)
        {
            int i = Location[0];
            int j = Location[1];

            //down 1
            if (i + 1 < 8)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i + 1, j] > 16) //if it is a black piece
                    {
                        validMoveLocations[i + 1, j] = true;
                    }
                    else if (pieces[i + 1, j] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 1, j] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i + 1, j] < 17 && pieces[i + 1, j] > 0) //if it is a white piece
                    {
                        validMoveLocations[i + 1, j] = true;
                    }
                    else if (pieces[i + 1, j] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 1, j] = true;
                    }
                }
            }
            //up 1
            if (i - 1 >= 0)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i - 1, j] > 16) //if it is a black piece
                    {
                        validMoveLocations[i - 1, j] = true;
                    }
                    else if (pieces[i - 1, j] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 1, j] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i - 1, j] < 17 && pieces[i - 1, j] > 0) //if it is a white piece
                    {
                        validMoveLocations[i - 1, j] = true;
                    }
                    else if (pieces[i - 1, j] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 1, j] = true;
                    }
                }
            }
            // right 1
            if (j + 1 < 8)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i, j + 1] > 16) //if it is a black piece
                    {
                        validMoveLocations[i, j + 1] = true;
                    }
                    else if (pieces[i, j + 1] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j + 1] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i, j + 1] < 17 && pieces[i, j + 1] > 0) //if it is a white piece
                    {
                        validMoveLocations[i, j + 1] = true;
                    }
                    else if (pieces[i, j + 1] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j + 1] = true;
                    }
                }
            }
            // left 1
            if (j - 1 >= 0)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i, j - 1] > 16) //if it is a black piece
                    {
                        validMoveLocations[i, j - 1] = true;
                    }
                    else if (pieces[i, j - 1] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j - 1] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i, j - 1] < 17 && pieces[i, j - 1] > 0) //if it is a white piece
                    {
                        validMoveLocations[i, j - 1] = true;
                    }
                    else if (pieces[i, j - 1] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i, j - 1] = true;
                    }
                }
            }
            // down 1 right 1
            if (i + 1 < 8 && j + 1 < 8)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i + 1, j + 1] > 16) //if it is a black piece
                    {
                        validMoveLocations[i + 1, j + 1] = true;
                    }
                    else if (pieces[i + 1, j + 1] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 1, j + 1] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i + 1, j + 1] < 17 && pieces[i + 1, j + 1] > 0) //if it is a white piece
                    {
                        validMoveLocations[i + 1, j + 1] = true;
                    }
                    else if (pieces[i + 1, j + 1] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 1, j + 1] = true;
                    }
                }
            }
            // down 1 left 1
            if (i + 1 < 8 && j - 1 >= 0)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i + 1, j - 1] > 16) //if it is a black piece
                    {
                        validMoveLocations[i + 1, j - 1] = true;
                    }
                    else if (pieces[i + 1, j - 1] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 1, j - 1] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i + 1, j - 1] < 17 && pieces[i + 1, j - 1] > 0) //if it is a white piece
                    {
                        validMoveLocations[i + 1, j - 1] = true;
                    }
                    else if (pieces[i + 1, j - 1] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i + 1, j - 1] = true;
                    }
                }
            }
            // up 1 right 1
            if (i - 1 >= 0 && j + 1 < 8)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i - 1, j + 1] > 16) //if it is a black piece
                    {
                        validMoveLocations[i - 1, j + 1] = true;
                    }
                    else if (pieces[i - 1, j + 1] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 1, j + 1] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i - 1, j + 1] < 17 && pieces[i - 1, j + 1] > 0) //if it is a white piece
                    {
                        validMoveLocations[i - 1, j + 1] = true;
                    }
                    else if (pieces[i - 1, j + 1] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 1, j + 1] = true;
                    }
                }
            }
            // up 1 left 1
            if (i - 1 >= 0 && j - 1 >= 0)
            {
                if (turn == 0)   //whites turn
                {
                    if (pieces[i - 1, j - 1] > 16) //if it is a black piece
                    {
                        validMoveLocations[i - 1, j - 1] = true;
                    }
                    else if (pieces[i - 1, j - 1] > 0)   //if it is a white piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 1, j - 1] = true;
                    }
                }
                else //blacks turn
                {
                    if (pieces[i - 1, j - 1] < 17 && pieces[i - 1, j - 1] > 0) //if it is a white piece
                    {
                        validMoveLocations[i - 1, j - 1] = true;
                    }
                    else if (pieces[i - 1, j - 1] > 16)   //if it is a black piece
                    {

                    }
                    else //it is a blank spot
                    {
                        validMoveLocations[i - 1, j - 1] = true;
                    }
                }
            }
        }
    }
    public class Pawn : Piece
    {
        public Pawn(int[,] loc) : base(loc)
        {

        }

        /// <summary>
        /// Function updates the valid move locations array with a true or false for each index. True means the
        /// object can move to that place and false means it cannot. 
        /// </summary>
        public void createDestination(int[,] pieces, int turn)
        {

            int i = Location[0];
            int j = Location[1];
            if (turn == 0) //whites turn
            {
                if (i == 1)  //first move
                {
                    if (pieces[i + 2, j] == 0)  //if it is empty can move forward 2
                    {
                        validMoveLocations[i + 2, j] = true;
                    }
                }
                if (i + 1 < 8)
                {
                    if (pieces[i + 1, j] == 0) //if it is empty can move forward 1
                    {
                        validMoveLocations[i + 1, j] = true;
                    }
                }
                if (i + 1 < 8 && j + 1 < 8)
                {
                    if (pieces[i + 1, j + 1] > 16)  //if a black piece is down and to the right
                    {
                        validMoveLocations[i + 1, j + 1] = true;
                    }
                }
                if (i + 1< 8 && j - 1 >= 0)
                {
                    if (pieces[i + 1, j - 1] > 16)  //if a black piece is down and to the left
                    {
                        validMoveLocations[i + 1, j - 1] = true;
                    }
                }
                //still needs to handle new piece if reaches end
            }
            else
            {
                if (i == 6)  //first move
                {
                    if (pieces[i - 2, j] == 0)  //if it is empty can move forward 2
                    {
                        validMoveLocations[i - 2, j] = true;
                    }
                }
                if (i - 1 >= 0)
                {
                    if (pieces[i - 1, j] == 0) //if it is empty can move forward 1
                    {
                        validMoveLocations[i - 1, j] = true;
                    }
                }
                if (i - 1 >= 0 && j + 1 < 8)
                {
                    if (pieces[i - 1, j + 1] < 17 && pieces[i - 1, j + 1] > 0)  //if a white piece is up and to the right
                    {
                        validMoveLocations[i - 1, j + 1] = true;
                    }
                }
                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    if (pieces[i - 1, j - 1] < 17 && pieces[i - 1, j - 1] > 0)  //if a white piece is up and to the left
                    {
                        validMoveLocations[i - 1, j - 1] = true;
                    }
                }
                //still needs to handle new piece if reaches end
            }
        }
    }
}