using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Chess
{
    public class Piece
    {
        public int[,] pieceLocation = new int[8,8]; //position of the piece on the board, first index is row, second is column.

        /// <summary>
        /// The constructor takes an integer array of the location of the piece. It then passes the value into the pieceLocation field. 
        /// </summary>
        /// <param name="location"></param>
        public Piece(int[,] location)
        {
            pieceLocation = location;
        }
        
        public void Move()
        {
            throw new NotImplementedException();
        }
        public void Capture()
        {
            throw new NotImplementedException();
        }
    }
    public class Rook : Piece
    {
        public bool[,] validMoveLocations = new bool[8, 8]; //2-D integer array denotes available locations for rook to move
        private int[] Location = new int[2];
        public Rook(int[,] loc) : base(loc)
        {
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (loc[i,j] == 1)
                    {
                        Location[0] = i; //row is stored in the first index of Location
                        Location[1] = j; //col is stored in the second index of Location
                    }
                }
            }
            
        }
           
        public void createDestination()
        {
            for (int i = 0; i < 8; i++) //for loop to set the row of the piece location to true for the move locations
            {
                validMoveLocations[Location[0], i] = true;
            }
            for (int j = 0; j < 8; j++) //for loop to set the column of the piece location to true for the move locations
            {
                validMoveLocations[j, Location[1]] = true;
            }

        }
        public void deleteDestination()
        {
            //deletes destination buttons after the piece is moved. 
        }
    }

    public class Bishop : Piece
    {
        public bool[,] validMoveLocations = new bool[8, 8]; //2-D integer array denotes available locations for rook to move
        private int[] Location = new int[2];
        public Bishop(int[,] loc) : base(loc)
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

        public void createDestination()
        {
            int i = Location[0];
            int j = Location[1];
            while (i < 8 && j < 8)
            {
                validMoveLocations[i, j] = true;
                i++;
                j++;
            }

            i = Location[0];
            j = Location[1];
            while (i >= 0 && j >= 0)
            {
                validMoveLocations[i, j] = true;
                i--;
                j--;
            }

            i = Location[0];
            j = Location[1];
            while (i < 8 && j >= 0)
            {
                validMoveLocations[i, j] = true;
                i++;
                j--;
            }

            i = Location[0];
            j = Location[1];
            while (i >= 0 && j < 8)
            {
                validMoveLocations[i, j] = true;
                i--;
                j++;
            }
        }
        public void deleteDestination()
        {
            //deletes destination buttons after the piece is moved. 
        }

    }
    public class Queen : Piece
    {
        public bool[,] validMoveLocations = new bool[8, 8]; //2-D integer array denotes available locations for rook to move
        private int[] Location = new int[2];
        public Queen(int[,] loc) : base(loc)
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

        public void createDestination()
        {
            int i = 0;
            int j = 0;
            for (i = 0; i < 8; i++) //for loop to set the row of the piece location to true for the move locations
            {
                validMoveLocations[Location[0], i] = true;
            }
            for (j = 0; j < 8; j++) //for loop to set the column of the piece location to true for the move locations
            {
                validMoveLocations[j, Location[1]] = true;
            }

            i = Location[0];
            j = Location[1];
            while (i < 8 && j < 8)
            {
                validMoveLocations[i, j] = true;
                i++;
                j++;
            }

            i = Location[0];
            j = Location[1];
            while (i >= 0 && j >= 0)
            {
                validMoveLocations[i, j] = true;
                i--;
                j--;
            }

            i = Location[0];
            j = Location[1];
            while (i < 8 && j >= 0)
            {
                validMoveLocations[i, j] = true;
                i++;
                j--;
            }

            i = Location[0];
            j = Location[1];
            while (i >= 0 && j < 8)
            {
                validMoveLocations[i, j] = true;
                i--;
                j++;
            }


        }
        public void deleteDestination()
        {
            //deletes destination buttons after the piece is moved. 
        }
    }
}
