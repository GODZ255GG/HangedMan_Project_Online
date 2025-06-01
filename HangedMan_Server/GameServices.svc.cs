using HangedMan_Server.Model.POCO;
using System.Collections.Generic;
using HangedMan_Server.Model.DTO;

namespace HangedMan_Server
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "GameServices" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione GameServices.svc o GameServices.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class GameServices : IGameServices
    {
        public Match CreateMatch(Match createMatch)
        {
            return MatchDTO.CreateMatch(createMatch);
        }

        public List<Match> GetMatchesPlayed(int playerID)
        {
            return MatchDTO.GetMatchesPlayed(playerID);
        }

        public List<Match> GetMatchList(int playerID)
        {
            return MatchDTO.GetMatchesAvaliables(playerID);
        }

        public bool InitMatch(int guestID, int matchID)
        {
            return MatchDTO.InitMatch(guestID, matchID);
        }

        public string GetGuestNickName(int playerID)
        {
            return MatchDTO.GetGuestNickName(playerID);
        }

        public bool IsThereGuest(int matchID)
        {
            return MatchDTO.IsThereGuest(matchID);
        }

        public bool LeaveMatch(int matchID)
        {
            return MatchDTO.LeaveMatch(matchID);
        }

        public bool FinishMatch(int matchID)
        {
            return MatchDTO.FinishMatch(matchID);
        }

        public char? GetGuestLetter(int matchID)
        {
            return MatchDTO.GetGuestLetter(matchID);
        }

        public int GetMatchStatus(int matchID)
        {
            return MatchDTO.GetMatchStatus(matchID);
        }

        public int GetRemainingAttempts(int matchID)
        {
            return MatchDTO.GetRemainingAttempts(matchID);
        }

        public bool UpdateCharBD(char letter, int matchID)
        {
            return MatchDTO.UpdateCharBD(letter, matchID);
        }

        public void UpdatePointsEarned(int matchID, int playerID)
        {
            MatchDTO.UpdatePointsEarned(matchID, playerID);
        }

        public void PenalizeAbandon(int playerID)
        {
            MatchDTO.PenalizeAbandon(playerID);
        }

        public bool UpdateRemainingAttempts(int remainingAttempts, int matchID)
        {
            return MatchDTO.UpdateRemainingAttempts(remainingAttempts, matchID);
        }

        public bool UpdateWinner(int playerID, int matchID)
        {
            return MatchDTO.UpdateWinner(playerID, matchID);
        }

        public int? GetWinnerID(int matchID)
        {
            return Model.DTO.MatchDTO.GetWinnerID(matchID);
        }
    }
}
