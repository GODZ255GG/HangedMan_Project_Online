using HangedMan_Server.Model.POCO;
using System;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;

namespace HangedMan_Server.Model.DTO
{
    public class PlayerDTO
    {
        public static bool RegisterPlayer(Player newPlayer)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContex = new DataContext(connection);
                dataContex.GetTable<Player>().InsertOnSubmit(newPlayer);
                dataContex.SubmitChanges();
                return true;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool EmailAlreadyRegistered(string email)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                bool emailRegistered = dataContext.GetTable<Player>().Any(pl => pl.Email == email);
                return emailRegistered;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool TelephoneAlreadyRegistered(string telephone)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                bool telephoneRegistered = dataContext.GetTable<Player>().Any(pl => pl.PhoneNumber == telephone);
                return telephoneRegistered;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool NicknameAlreadyRegistered(string nickname)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                bool nicknameRegistered = dataContext.GetTable<Player>().Any(pl => pl.NickName == nickname);
                return nicknameRegistered;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static Player LogIn(string email, string password)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var user = (from us in dataContext.GetTable<Player>()
                            where us.Email == email && us.Password == password
                            select us).FirstOrDefault();
                return user;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static bool UpdatePlayerProfile(Player updatedPlayer)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var playerToUpdate = dataContext.GetTable<Player>().FirstOrDefault(pl => pl.PlayerID == updatedPlayer.PlayerID);

                if (playerToUpdate != null)
                {
                    playerToUpdate.FullName = updatedPlayer.FullName;
                    playerToUpdate.NickName = updatedPlayer.NickName;
                    playerToUpdate.Email = updatedPlayer.Email;
                    playerToUpdate.PhoneNumber = updatedPlayer.PhoneNumber;
                    playerToUpdate.Password = updatedPlayer.Password;
                    playerToUpdate.BirthDate = updatedPlayer.BirthDate;

                    dataContext.SubmitChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar el perfil del jugador: {ex.Message}");
                return false;
            }
        }

        public static int GetPoints(int playerID)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var pointEarned = (from p in dataContext.GetTable<Player>()
                                   where p .PlayerID == playerID
                                   select p .PointsEarned).FirstOrDefault();
                return pointEarned;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

    }
}