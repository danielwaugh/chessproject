namespace Chess
{
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