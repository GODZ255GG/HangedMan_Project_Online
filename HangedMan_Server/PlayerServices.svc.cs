using HangedMan_Server.Model.POCO;
using HangedMan_Server.Model.DTO;

namespace HangedMan_Server
{
    public class PlayerServices : IPlayerServices
    {
        public bool RegisterPlayer(Player newPlayer)
        {
            return PlayerDTO.RegisterPlayer(newPlayer);
        }

        public bool EmailAlreadyRegistered(string email)
        {
            return PlayerDTO.EmailAlreadyRegistered(email);
        }

        public bool NicknameAlreadyRegistered(string nickname)
        {
            return PlayerDTO.NicknameAlreadyRegistered(nickname);
        }

        public bool TelephoneAlreadyExist(string telephone)
        {
            return PlayerDTO.TelephoneAlreadyRegistered(telephone);
        }

        public Player LogIn(string email, string password)
        {
            return PlayerDTO.LogIn(email, password);
        }

        public bool UpdatePlayerProfile(Player updatePlayer)
        {
            return PlayerDTO.UpdatePlayerProfile(updatePlayer);
        }

        public int GetPoints(int playerID)
        {
            return PlayerDTO.GetPoints(playerID);
        }
    }
}
