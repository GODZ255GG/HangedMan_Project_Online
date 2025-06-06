using Logic.POCO;
using System.ServiceModel;

namespace Services
{
    /*
    * Fecha creación: 31/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Interfaz de contrato WCF para la gestión de jugadores en el juego "Ahorcado".
    *              Define las operaciones disponibles para el registro, autenticación, actualización de perfil y consulta de información de los jugadores.
    */
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
