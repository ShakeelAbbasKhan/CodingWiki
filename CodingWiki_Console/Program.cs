// See https://aka.ms/new-console-template for more information
using CodingWiki_DataAccess.Data;
using CodingWiki_Model.Models;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;

Console.WriteLine("Hello, World!");

//using(ApplicationDbContext context = new ApplicationDbContext())
//{
//    context.Database.EnsureCreated();
//    if(context.Database.GetPendingMigrations().Count() > 0 )
//    {
//        context.Database.Migrate();
//    }
//}

//AddBook();
//UpdateBook();
//DeleteBook();

//void DeleteBook()
//{
//    try
//    {
//        using var context = new ApplicationDbContext();
//        var book = context.Books.Find(7);
//        context.Remove(book);
//        context.SaveChanges();
//    }
//    catch (Exception ex)
//    {

//    }
//}

//void UpdateBook()
//{
//    try
//    {
//        using var context = new ApplicationDbContext();
//        var book = context.Books.Find(7);
//        book.ISBN = "777";
//        context.SaveChanges();
//    }
//    catch(Exception ex)
//    {

//    }
//}

//GetAllBooks();
// GetBook
// GetPractice();

//void GetPractice()
//{
//    using var context = new ApplicationDbContext();
//    // var books = context.Books.Where(u=>u.Publisher_Id == 2  && u.Price > 20);   // Filter by where
//    // var books = context.Books.Where(u => u.ISBN.Contains("12"));  // contain mehtod return record where 12 exitst and work as like operator
//    // var books = context.Books.Where(u =>EF.Functions.Like(u.ISBN,"12%"));  //  return record where record start from 12
//    var books = context.Books.OrderBy(u => u.Title).ThenByDescending(u=>u.ISBN);  // first ordy by asc title and desc by isbn


//    foreach (var book in books)
//    {
//        Console.WriteLine(book.Title + "-" + book.ISBN);
//    }
//    //Console.WriteLine(book.Title + "-" + book.ISBN);  // use where firstOrDefualt use and return one record


//    var booksPagination = context.Books.Skip(0).Take(2);  // first ordy by asc title and desc by isbn
//    foreach (var book in booksPagination)
//    {
//        Console.WriteLine(book.Title + "-" + book.ISBN);
//    }
//    booksPagination = context.Books.Skip(4).Take(1);  // pagination
//    foreach (var book in booksPagination)
//    {
//        Console.WriteLine(book.Title + "-" + book.ISBN);
//    }
//}

//void GetBook()
//{
//    using var context = new ApplicationDbContext();
//    var book = context.Books.Where(u => u.Publisher_Id == 2).FirstOrDefault();
//    Console.WriteLine(book.Title + "-" + book.ISBN);  // use where firstOrDefualt use and return one record

//}

//void GetAllBooks()
//{
//    using var context = new ApplicationDbContext();
//    var books = context.Books.ToList();
//    foreach (var book in books)
//    {
//        Console.WriteLine(book.Title + "-" + book.ISBN);
//    }
//}
//void AddBook()
//{
//    Book book = new()
//    {
//        Title = "New EF Core Book",
//        ISBN = "9999",
//        Price = 120,
//        Publisher_Id = 1
//    };

//    using var context = new ApplicationDbContext();
//    context.Books.Add(book);
//    context.SaveChanges();
//}