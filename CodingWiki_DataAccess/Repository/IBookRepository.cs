using CodingWiki_Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace CodingWiki_DataAccess.Repository
{
    public interface IBookRepository
    {
        public Task<List<Book>> GetAllBooks();
        public Task<List<Publisher>> GetPublishers();
        public Task<Book> CreateAndUpdateBooks(Book book);
    }
}
