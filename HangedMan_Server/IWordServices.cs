using HangedMan_Server.Model.POCO;
using System.Collections.Generic;
using System.ServiceModel;

namespace HangedMan_Server
{
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
