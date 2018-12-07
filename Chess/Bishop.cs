namespace Chess
{
    public class Bishop : Piece
    {
        public Bishop(int[,] loc) : base(loc)
        {

        }

        /// <summary>
        /// Function updates the valid move locations array with a true or false for each index. True means the
        /// object can move to that place and false means it can't. 
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