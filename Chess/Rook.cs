namespace Chess
{
    public class Rook : Piece
    {
        public Rook(int[,] loc) : base(loc)
        {

        }

        public void CreateDestination(int[,] pieces, int turn)
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