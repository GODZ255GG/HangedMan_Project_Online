using Logic.DTO;
using Logic.POCO;
using System.Collections.Generic;

namespace Services
{
    /*
    * Fecha creación: 31/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Implementación de los servicios WCF para el juego "Ahorcado".
    *              Expone operaciones para la gestión de jugadores, partidas y palabras, permitiendo la interacción entre clientes y la lógica del juego.
    */
    public partial class HangedManService : IPlayerServices
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

    public partial class HangedManService : IGameServices
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
            return MatchDTO.GetWinnerID(matchID);
        }
    }

    public partial class HangedManService : IWordServices
    {
        public List<Category> GetCategories()
        {
            return CategoryDTO.GetCategories();
        }

        public string GetClueEnglish(int wordID)
        {
            return WordDTO.GetClueEnglish(wordID);
        }

        public string GetClueSpanish(int wordID)
        {
            return WordDTO.GetClueSpanish(wordID);
        }

        public string GetWordEnglish(int wordID)
        {
            return WordDTO.GetWordEnglish(wordID);
        }

        public string GetWordSpanish(int wordID)
        {
            return WordDTO.GetWordSpanish(wordID);
        }

        public List<Word> GetWordsPerCategory(int category)
        {
            return WordDTO.GetWordsPerCategory(category);
        }

        public string GetCategoryByWordID(int wordID, int matchLanguage)
        {
            return WordDTO.GetCategoryByWordID(wordID, matchLanguage);
        }

    }
}
