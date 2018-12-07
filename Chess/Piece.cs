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

        public override bool Equals(object obj)
        {
            var piece = obj as Piece;
            return piece != null &&
                   EqualityComparer<int[]>.Default.Equals(Location, piece.Location) &&
                   EqualityComparer<bool[,]>.Default.Equals(validMoveLocations, piece.validMoveLocations);
        }

        public override int GetHashCode()
        {
            var hashCode = -1494653214;
            hashCode = hashCode * -1521134295 + EqualityComparer<int[]>.Default.GetHashCode(Location);
            hashCode = hashCode * -1521134295 + EqualityComparer<bool[,]>.Default.GetHashCode(validMoveLocations);
            return hashCode;
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}