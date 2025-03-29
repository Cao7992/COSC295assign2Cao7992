using COSC295assign2Cao7992.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;


namespace COSC295assign2Cao7992
{
    public class Database
    {
        readonly SQLiteConnection database;
        public Database(string dbPath)
        {
            // initialize database and create tables inside
            database = new SQLiteConnection(dbPath);
            database.CreateTable<Opponent>();
            database.CreateTable<Match>();
            database.CreateTable<Game>();
            hardcodeGameTable();
        }

        // Opponent Methods
        // method is get all Opponents, return as a list
        public List<Opponent> GetOpponents() => database.Table<Opponent>().ToList();
        // method to get 1 Opponent based on id parameter
        public Opponent GetOpponent(int id) => database.Table<Opponent>().FirstOrDefault(o => o.ID == id);
        // method to insert new Opponent into table
        public int SaveOpponent(Opponent opponent) => database.Insert(opponent);
        // method to delete Opponent based on id parameter and all of its related matches
        public int DeleteOpponent(int id)
        {
            var opponent = GetOpponent(id);
            if (opponent != null)
            {
                // Delete associated matches first
                var matches = GetMatchesByOpponent(id);
                foreach (var match in matches)
                {
                    database.Delete(match);
                }
                return database.Delete(opponent);
            }
            return 0;
        }

        // Match Methods
        // method is get all Matches, return as a list
        public List<Match> GetMatches() => database.Table<Match>().ToList();
        // method to get all matches of a specific Opponent based on opponentID parameter
        public List<Match> GetMatchesByOpponent(int opponentId) => database.Table<Match>().Where(m => m.OpponentID == opponentId).ToList();
        // method to get 1 Match based on id parameter
        public Match GetMatch(int id) => database.Table<Match>().FirstOrDefault(m => m.ID == id);
        // method to insert new Match into Match table
        public int SaveMatch(Match match) => database.Insert(match);
        // method to update a Match in Match table
        public int UpdateMatch(Match match) => database.Update(match);

        public int DeleteMatch(int id)
        {
            var match = GetMatch(id);
            return match != null ? database.Delete(match) : 0;
        }

        // Game Methods
        // method to get all Games, return as a list
        public List<Game> GetGames() => database.Table<Game>().ToList();
        // method to get 1 Game based on id parameter
        public Game GetGame(int id) => database.Table<Game>().FirstOrDefault(g => g.ID == id);
        // method to insert new Game into Game table
        public int SaveGame(Game game) => database.Insert(game);

        // Reset Database
        public void ResetDatabase()
        {
            database.DropTable<Opponent>();
            database.DropTable<Match>();
            database.DropTable<Game>();
            database.CreateTable<Opponent>();
            database.CreateTable<Match>();
            database.CreateTable<Game>();

            // Insert default game records
            hardcodeGameTable();
        }

        private void hardcodeGameTable()
        {
            database.Insert(new Game { GameName = "Chess", Description = "Simple grid game", Rating = 9.5 });
            database.Insert(new Game { GameName = "Checkers", Description = "Simpler grid game", Rating = 5 });
            database.Insert(new Game { GameName = "Dominoes", Description = "Blocks game", Rating = 6.75 });
        }
    }
}
