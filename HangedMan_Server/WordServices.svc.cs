using HangedMan_Server.Model.DTO;
using HangedMan_Server.Model.POCO;
using System.Collections.Generic;

namespace HangedMan_Server
{
    // NOTA: puede usar el comando "Rename" del menú "Refactorizar" para cambiar el nombre de clase "WordServices" en el código, en svc y en el archivo de configuración a la vez.
    // NOTA: para iniciar el Cliente de prueba WCF para probar este servicio, seleccione WordServices.svc o WordServices.svc.cs en el Explorador de soluciones e inicie la depuración.
    public class WordServices : IWordServices
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
            return Model.DTO.WordDTO.GetCategoryByWordID(wordID, matchLanguage);
        }

    }
}
