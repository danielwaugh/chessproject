using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Chess
{ //look up uwp file access issues. Might reference email
    /// <summary>
    /// SaveState Class utilizes Newtonsoft.Json to serialize and deserialize data. This is
    /// used to save the state of the current game to be returned to later. 
    /// </summary>
    public class SaveState
    {
        private string BoardToJson { get; set; } //Json Serialized string
        void Save(Board SaveThis) //Method that saves the current board passed in
        {
            BoardToJson = JsonConvert.SerializeObject(SaveThis);
        }
        Board Load(Board loadBoard) //Method that restores the board
        {
            Board JsonToBoard = JsonConvert.DeserializeObject<Board>(BoardToJson);
            return JsonToBoard;
        }
    }
}
