using Logic.POCO;
using System.Collections.Generic;
using System.ServiceModel;

namespace Services
{
    /*
    * Fecha creación: 31/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Interfaz de contrato WCF para la gestión de partidas en el juego "Ahorcado".
    *              Define las operaciones disponibles para la creación, administración y control de partidas entre los clientes y el servidor.
    */
    [ServiceContract]
    public interface IGameServices
    {
        [OperationContract]
        Match CreateMatch(Match createMatch);
        [OperationContract]
        List<Match> GetMatchList(int playerID);
        [OperationContract]
        List<Match> GetMatchesPlayed(int playerID);
        [OperationContract]
        bool InitMatch(int guestID, int matchID);
        [OperationContract]
        string GetGuestNickName(int playerID);
        [OperationContract]
        bool IsThereGuest(int matchID);
        [OperationContract]
        bool LeaveMatch(int matchID);
        [OperationContract]
        char? GetGuestLetter(int matchID);
        [OperationContract]
        int GetRemainingAttempts(int matchID);
        [OperationContract]
        void UpdatePointsEarned(int matchID, int playerID);
        [OperationContract]
        void PenalizeAbandon(int playerID);
        [OperationContract]
        bool FinishMatch(int matchID);
        [OperationContract]
        bool UpdateCharBD(char letter, int matchID);
        [OperationContract]
        int GetMatchStatus(int matchID);
        [OperationContract]
        bool UpdateRemainingAttempts(int remainingAttempts, int matchID);
        [OperationContract]
        bool UpdateWinner(int playerID, int matchID);
        [OperationContract]
        int? GetWinnerID(int matchID);
    }
}
