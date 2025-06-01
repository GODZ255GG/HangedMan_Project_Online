using HangedMan_Server.Model.POCO;
using System.ServiceModel;

namespace HangedMan_Server
{
    [ServiceContract]
    public interface IPlayerServices
    {
        [OperationContract]
        bool RegisterPlayer(Player newPlayer);
        [OperationContract]
        bool EmailAlreadyRegistered(string email);
        [OperationContract]
        bool NicknameAlreadyRegistered(string nickname);
        [OperationContract]
        bool TelephoneAlreadyExist(string telephone);
        [OperationContract]
        Player LogIn(string email, string password);
        [OperationContract]
        bool UpdatePlayerProfile(Player updatePlayer);
        [OperationContract]
        int GetPoints(int playerID);
    }
}
