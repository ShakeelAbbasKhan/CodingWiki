using CodingWiki_DataAccess.Data;
using CodingWiki_Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodingWiki_DataAccess.Repository
{
    
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _db;

        public BookRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<Book> CreateAndUpdateBooks(Book book)
        {
            //Book book1 = new Book()
            //{
            //    Title = book.Title,
            //    Price = book.Price,
            //    ISBN = book.ISBN,
            //};
            if (book.BookId == 0)
            {
                // Create
                await _db.Books.AddAsync(book);
            }
            else
            {
                // Edit
                _db.Books.Update(book);
            }

            await _db.SaveChangesAsync();
            return book;
        }

        public async Task<List<Book>> GetAllBooks()
        {
            return await _db.Books.Include(u => u.Publisher).Include(u => u.BookAuthorMap).ThenInclude(u => u.Author).ToListAsync();
        }

        public async Task<List<Publisher>> GetPublishers()
        {
            return await _db.Publishers.ToListAsync();
        }
    }
}
