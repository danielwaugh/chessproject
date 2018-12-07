namespace Chess
{
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

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}