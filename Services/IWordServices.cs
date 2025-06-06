using Logic.POCO;
using System.Collections.Generic;
using System.ServiceModel;

namespace Services
{
    /*
    * Fecha creación: 31/05/2025
    * Última modificación: 06/06/2025
    * Último modificador: René Ulises
    * Descripción: Interfaz de contrato WCF para la gestión de palabras y categorías en el juego "Ahorcado".
    *              Define las operaciones disponibles para consultar palabras, pistas y categorías, permitiendo la interacción entre los clientes y el servidor respecto al contenido del juego.
    */
    [ServiceContract]
    public interface IWordServices
    {
        [OperationContract]
        List<Category> GetCategories();
        [OperationContract]
        List<Word> GetWordsPerCategory(int category);
        [OperationContract]
        string GetWordSpanish(int wordID);
        [OperationContract]
        string GetWordEnglish(int wordID);
        [OperationContract]
        string GetClueSpanish(int wordID);
        [OperationContract]
        string GetClueEnglish(int wordID);
        [OperationContract]
        string GetCategoryByWordID(int wordID, int matchLanguage);
    }
}
