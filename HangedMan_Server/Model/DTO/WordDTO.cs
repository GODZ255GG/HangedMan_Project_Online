using HangedMan_Server.Model.POCO;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;

namespace HangedMan_Server.Model.DTO
{
    public class WordDTO
    {
        public static List<Word> GetWordsPerCategory(int category)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var words = (from wor in dataContext.GetTable<Word>()
                             where wor.CategoryID == category
                             select wor).ToList();
                return words;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static string GetWordSpanish(int wordID)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var word = (from wor in dataContext.GetTable<Word>()
                            where wor.WordID == wordID
                            select wor.SpanishWord).FirstOrDefault();
                return word;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static string GetWordEnglish(int wordID)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var word = (from wor in dataContext.GetTable<Word>()
                            where wor.WordID == wordID
                            select wor.EnglishWord).FirstOrDefault();
                return word;

            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static string GetClueSpanish(int wordID)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var clue = (from clu in dataContext.GetTable<Word>()
                            where clu.WordID == wordID
                            select clu.SpanishClue).FirstOrDefault();
                return clue;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static string GetClueEnglish(int wordID)
        {
            try
            {
                var connection = ConnectionDB.getConnection();
                connection.Open();
                DataContext dataContext = new DataContext(connection);
                var clue = (from clu in dataContext.GetTable<Word>()
                            where clu.WordID == wordID
                            select clu.EnglishClue).FirstOrDefault();
                return clue;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        public static string GetCategoryByWordID(int wordID, int matchLanguage)
        {
            try
            {
                using (var connection = ConnectionDB.getConnection())
                {
                    connection.Open();
                    DataContext dataContext = new DataContext(connection);
                    var query = from w in dataContext.GetTable<Word>()
                                join c in dataContext.GetTable<Category>() on w.CategoryID equals c.CategoryID
                                where w.WordID == wordID
                                select c;
                    var category = query.FirstOrDefault();
                    if (category == null)
                        return null;
                    return matchLanguage == 1 ? category.SpanishCategory : category.EnglishCategory;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
    }
}
