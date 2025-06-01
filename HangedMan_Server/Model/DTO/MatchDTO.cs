using HangedMan_Server.Model.POCO;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;

namespace HangedMan_Server.Model.DTO
{
    public class MatchDTO
    {
        public static Match CreateMatch(Match newMatch)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                dataContext.GetTable<Match>().InsertOnSubmit(newMatch);
                dataContext.SubmitChanges();
                return newMatch;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static List<Match> GetMatchesAvaliables(int playerID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);
                    var matches = (from mat in dataContext.GetTable<Match>()
                                   where mat.StatusMatchID == 1 && mat.ChallengerID != playerID
                                   select mat).ToList();
                    return matches;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static List<Match> GetMatchesPlayed(int playerID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);

                    var matches = (from mat in dataContext.GetTable<Match>()
                                   where (mat.StatusMatchID == 2 || mat.StatusMatchID == 3) &&
                                         (mat.ChallengerID == playerID || mat.GuestID == playerID)
                                   orderby mat.DateMatch descending
                                   select mat).ToList();

                    return matches;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool InitMatch(int guestID, int matchID)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var matchToUpdate = dataContext.GetTable<Match>().FirstOrDefault(mat => mat.MatchID == matchID);
                if (matchToUpdate != null)
                {
                    matchToUpdate.GuestID = guestID;
                    matchToUpdate.StatusMatchID = 4;
                    dataContext.SubmitChanges();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static string GetGuestNickName(int playerID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);
                    var nickName = (from nic in dataContext.GetTable<Player>()
                                    where nic.PlayerID == playerID
                                    select nic.NickName).FirstOrDefault();
                    return nickName;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool LeaveMatch(int matchID)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var matchToLeave = dataContext.GetTable<Match>().FirstOrDefault(mat => mat.MatchID == matchID);
                if (matchToLeave != null)
                {
                    matchToLeave.StatusMatchID = 2;
                    dataContext.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool IsThereGuest(int matchID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);
                    var guest = (from gue in dataContext.GetTable<Match>()
                                 where gue.MatchID == matchID && gue.GuestID != null
                                 select gue.GuestID).FirstOrDefault();
                    return guest != null;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool FinishMatch(int matchID)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var matchToLeave = dataContext.GetTable<Match>().FirstOrDefault(mat => mat.MatchID == matchID);
                if (matchToLeave != null)
                {
                    matchToLeave.StatusMatchID = 3;
                    dataContext.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }

        }

        public static bool UpdateCharBD(char letter, int matchID)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var charUpdate = dataContext.GetTable<Match>().FirstOrDefault(mat => mat.MatchID == matchID);
                if (charUpdate != null)
                {
                    charUpdate.SelectedLetter = letter;
                    dataContext.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool UpdateWinner(int playerID, int matchID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);
                    var updateWinner = dataContext.GetTable<Match>().FirstOrDefault(mat => mat.MatchID == matchID);
                    if (updateWinner != null)
                    {
                        updateWinner.WinnerID = playerID;
                        dataContext.SubmitChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool UpdateRemainingAttempts(int remainingAttempts, int matchID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);
                    var updateAttempts = dataContext.GetTable<Match>().FirstOrDefault(mat => mat.MatchID == matchID);
                    if (updateAttempts != null)
                    {
                        updateAttempts.RemainingAttempts = remainingAttempts;
                        dataContext.SubmitChanges();
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static int GetMatchStatus(int matchID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);
                    var status = (from sta in dataContext.GetTable<Match>()
                                  where sta.MatchID == matchID
                                  select sta.StatusMatchID).FirstOrDefault();
                    return status;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static char? GetGuestLetter(int matchID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);
                    var letter = (from lett in dataContext.GetTable<Match>()
                                  where lett.MatchID == matchID && lett.GuestID != null
                                  select lett.SelectedLetter).FirstOrDefault();
                    return letter;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static int GetRemainingAttempts(int matchID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);
                    var attempts = (from at in dataContext.GetTable<Match>()
                                    where at.MatchID == matchID
                                    select at.RemainingAttempts).FirstOrDefault();
                    return attempts;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static void UpdatePointsEarned(int matchID, int playerID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);

                    var match = (from m in dataContext.GetTable<Match>()
                                 where m.MatchID == matchID
                                 select m).FirstOrDefault();

                    if (match == null || match.WinnerID == null)
                        return;

                    var player = (from p in dataContext.GetTable<Player>()
                                  where p.PlayerID == playerID
                                  select p).FirstOrDefault();

                    if (player == null)
                        return;

                    if (match.WinnerID == match.ChallengerID && playerID == match.ChallengerID)
                    {
                        player.PointsEarned += 5;
                    }
                    else if (match.WinnerID == match.GuestID && playerID == match.GuestID)
                    {
                        player.PointsEarned += 10;
                    }

                    dataContext.SubmitChanges();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static void PenalizeAbandon(int playerID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);

                    var player = (from p in dataContext.GetTable<Player>()
                                  where p.PlayerID == playerID
                                  select p).FirstOrDefault();

                    if (player == null)
                        return;

                    player.PointsEarned -= 3;
                    dataContext.SubmitChanges();
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }


        public static int? GetWinnerID(int matchID)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);
                    var winnerId = (from mat in dataContext.GetTable<Match>()
                                    where mat.MatchID == matchID
                                    select mat.WinnerID).FirstOrDefault();
                    return winnerId;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}