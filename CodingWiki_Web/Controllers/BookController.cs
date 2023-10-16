using CodingWiki_DataAccess.Data;
using CodingWiki_DataAccess.Repository;
using CodingWiki_Model.Models;
using CodingWiki_Model.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CodingWiki_Web.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IBookRepository _bookRepository;

        public BookController(ApplicationDbContext db,IBookRepository bookRepository)
        {
            _db = db;
            _bookRepository = bookRepository;
        }
        public async Task<IActionResult> Index()
        {
            // done by eager loading

            var bookList = await _bookRepository.GetAllBooks();

            //List<Book> bookList = _db.Books.Include(u=>u.Publisher).Include(u=>u.BookAuthorMap).ThenInclude(u=>u.Author).ToList();

            //List<Book> bookList = _db.Books.ToList();

            // from below code more efficient is to use include  which bring in one query

            //foreach (var obj in bookList)
            //{
            //    // least efficient
            //    //obj.Publisher = _db.Publishers.FirstOrDefault(u=>u.Publisher_Id == obj.Publisher_Id);    // show data to publisher name and for every row query run an other way is explicit loading
            //    // most efficient
            //    _db.Entry(obj).Reference(u => u.Publisher).Load();
            //    _db.Entry(obj).Collection(u => u.BookAuthorMap).Load();
            //    foreach (var bookAuth in obj.BookAuthorMap)
            //    {
            //        _db.Entry(bookAuth).Reference(u => u.Author).Load();
            //    }
            //}
            return View(bookList);
        }

        public IActionResult Upsert(int? id)
        {
            BookVM obj = new BookVM();

            //obj.PublisherList = _db.Publishers.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value  = i.Publisher_Id.ToString(),
            //});
            var publishers = _bookRepository.GetPublishers().Result;
            obj.PublisherList = publishers.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Publisher_Id.ToString(),
            });

            if (id == null || id == 0)
            {
                //create
                return View(obj);
            }
            // edit
            var books = _bookRepository.GetAllBooks().Result;
            obj.Book = books.FirstOrDefault(u => u.BookId == id);
            //obj.Book = _db.Books.FirstOrDefault(u => u.BookId== id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(BookVM obj)
        {
                if (obj.Book.BookId == 0)
                {
                    // create
                    await _db.Books.AddAsync(obj.Book);

                }
                else
                {
                    // edit
                    _db.Books.Update(obj.Book);
                }
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
           
        }

        // first method for details
        public IActionResult Details(int? id)
        {
            BookVM obj = new BookVM();

            if (id == null || id == 0)
            {
                return NotFound();
            }
            // edit
            obj.Book = _db.Books.FirstOrDefault(u => u.BookId == id);
            obj.Book.BookDetail = _db.BookDetails.FirstOrDefault(u => u.Book_Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(BookVM obj)
        {
            obj.Book.BookDetail.Book_Id = obj.Book.BookId;
            if (obj.Book.BookDetail.BookDetail_Id == 0)
            {
                // create
                await _db.BookDetails.AddAsync(obj.Book.BookDetail);

            }
            else
            {
                // edit
                _db.BookDetails.Update(obj.Book.BookDetail);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // 2nd method
        //public IActionResult Details(int? id)
        //{
        //    if (id == null || id == 0)
        //    {
        //        return NotFound();
        //    }
        //    BookDetail obj = new();

        //    //edit
         
        //    obj = _db.BookDetails.Include(u => u.Book).FirstOrDefault(u => u.Book_Id == id);
        //    if (obj == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(obj);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Details(BookDetail obj)
        //{
        //    if (obj.BookDetail_Id == 0)
        //    {
        //        //create
        //        await _db.BookDetails.AddAsync(obj);
        //    }
        //    else
        //    {
        //        //update
        //        _db.BookDetails.Update(obj);
        //    }
        //    await _db.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        public IActionResult ManageAuthors(int id)
        {
            BookAuthorVM obj = new BookAuthorVM()
            {
                BookAuthorList = _db.BookAuthorMaps.Include(u=>u.Author).Include(u=>u.Book).Where(u => u.Book_Id == id).ToList(),
                BookAuthor = new()
                {
                    Book_Id = id
                },
                Book = _db.Books.FirstOrDefault(u=>u.BookId==id)   // it is b/c when bookauthlist is empty
            };

            List<int> tempListOfAssignedAuthor = obj.BookAuthorList.Select(u=>u.Author_Id).ToList();
            //NOT IN clause
            //get all the authors whos id is not in tempListOfAssignedAuthors

            var tempList = _db.Authors.Where(u => !tempListOfAssignedAuthor.Contains(u.Author_Id)).ToList();
            obj.AuthorList = tempList.Select(i => new SelectListItem
            {
                Text = i.FullName,
                Value = i.Author_Id.ToString()
            });

            return View(obj);

        }


        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {
            if (bookAuthorVM.BookAuthor.Book_Id != 0 && bookAuthorVM.BookAuthor.Author_Id != 0)
            {
                _db.BookAuthorMaps.Add(bookAuthorVM.BookAuthor);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.Book_Id });
        }

        [HttpPost]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM)
        {
            int bookId = bookAuthorVM.Book.BookId;
            BookAuthorMap bookAuthorMap = _db.BookAuthorMaps.FirstOrDefault(
                u => u.Author_Id == authorId && u.Book_Id == bookId);


            _db.BookAuthorMaps.Remove(bookAuthorMap);
            _db.SaveChanges();
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            Book obj = new Book();
            obj = _db.Books.FirstOrDefault(u => u.BookId == id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Books.Remove(obj);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // Deffered Execution
        public async Task<IActionResult> Playground()
        {
            // Diff between IEnumerable & IQueryable 1.IEnumerable not filter data in query it filter in memory but IQueryable filter in query
            IEnumerable<Book> BookList1 = _db.Books;
            var FilteredBook1 = BookList1.Where(b => b.Price > 50).ToList();

            IQueryable<Book> BookList2 = _db.Books;
            var fileredBook2 = BookList2.Where(b => b.Price > 50).ToList();


            //var bookTemp = _db.Books.FirstOrDefault();
            //bookTemp.Price = 100;

            //var bookCollection = _db.Books;
            //decimal totalPrice = 0;

            //foreach (var book in bookCollection)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookList = _db.Books.ToList();
            //foreach (var book in bookList)
            //{
            //    totalPrice += book.Price;
            //}

            //var bookCollection2 = _db.Books;
            //var bookCount1 = bookCollection2.Count();

            //var bookCount2 = _db.Books.Count();
            return RedirectToAction(nameof(Index));
        }
    }
}
