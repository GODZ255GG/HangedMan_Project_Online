using System.Collections.Generic;
using HangedMan_Server.Model.POCO;
using System.ServiceModel;

namespace HangedMan_Server
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de interfaz "IGameServices" en el código y en el archivo de configuración a la vez.
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
