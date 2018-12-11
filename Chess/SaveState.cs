using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{ //look up uwp file access issues. Might reference email
    /// <summary>
    /// SaveState Class utilizes Newtonsoft.Json to serialize and deserialize data. This is
    /// used to save the state of the current game to be returned to later. 
    /// </summary>
    public class SaveState
    {
        private Board saveBoard = new Board(); //Json Serialized string
        public void Save(Board SaveThis) //Method that saves the current board passed in
        {
            saveBoard = SaveThis;
        }
        public Board Load() //Method that restores the board
        {
            try
            {
                return saveBoard;
            }
            catch (System.ArgumentNullException)
            {
                return null;
            }
        }
    }
}
