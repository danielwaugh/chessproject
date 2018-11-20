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
        private int[,] validMoveLocations = new int[8, 8]; //2-D integer array denotes available locations for rook to move
        private int[] Location = new int[2];
        public Rook(int[,] loc) : base(loc)
        {
            
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (loc[i,j] == 1)
                    {
                        Location[1] = i; //row is stored in the first index of Location
                        Location[2] = j; //col is stored in the second index of Location
                    }
                }
            }
            
        }
           
        public void createDestination()
        {
            //creates destination buttons for available locations to move to
        }
        public void deleteDestination()
        {
            //deletes destination buttons after the piece is moved. 
        }
    }
}
