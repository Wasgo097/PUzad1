using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;

namespace RepositoryPattern
{
    public class LibraryRepository
    {
        private Database db { get; }
        public LibraryRepository(Database db)
        {
            this.db = db;
        }
        public List<BookDTO> GetBooks(PaginationDTO pagination)
        {
            return db.Books.
                Include(b => b.Rates).
                Include(b => b.Authors).
                Skip(pagination.Count * pagination.Page).
                Take(pagination.Count).
                ToList().Select
                (b => new BookDTO
                {
                    Id = b.Id,
                    ReleaseDate = b.ReleaseDate,
                    AvarageRate = b.Rates.Count > 0 ? b.Rates.Average(r => r.Value) : 0,
                    RatesCount = b.Rates.Count(),
                    Title = b.Title,
                    Authors = b.Authors.Select(a => new BookAuthorDTO
                    {
                        FirstName = a.FirstName,
                        Id = a.Id,
                        SecondName = a.SecondName
                    }).ToList()
                }).ToList();
        }
        public BookDTO GetBook(int id)
        {
            return db.Books.
                Include(b => b.Rates).
                Include(b => b.Authors).
                ToList().Select(b => new BookDTO
                {
                    Id = b.Id,
                    ReleaseDate = b.ReleaseDate,
                    AvarageRate = b.Rates.Count>0? b.Rates.Average(r => r.Value):0,
                    RatesCount = b.Rates.Count(),
                    Title = b.Title,
                    Authors = b.Authors.Select(a => new BookAuthorDTO
                    {
                        FirstName = a.FirstName,
                        Id = a.Id,
                        SecondName = a.SecondName
                    }).ToList()
                }).Where(b => b.Id == id).Single();
        }
        public void PostBook(BookRequestDTO bookdto)
        {
            Book newbook = new Book
            {
                Title = bookdto.Title,
                ReleaseDate = bookdto.ReleaseDate,

            };
            newbook.Authors = db.Authors.Where(a => bookdto.AuthorsIds.Contains(a.Id)).ToList();
            db.Books.Add(newbook);
            db.SaveChanges();
            //return new BookDTO
            //{
            //    Id = newbook.Id,
            //    Authors = newbook.Authors.Select(a => new BookAuthorDTO
            //    {
            //        Id = a.Id,
            //        FirstName = a.FirstName,
            //        SecondName = a.SecondName

            //    }).ToList(),
            //    AvarageRate = 0,
            //    RatesCount = 0,
            //    ReleaseDate = newbook.ReleaseDate,
            //    Title = newbook.Title
            //};
        }
        public bool DeleteBook(int index)
        {
            var book = db.Books.Where(x => x.Id == index).Single();
            if (book != null)
            {
                db.Books.Remove(book);
                db.SaveChanges();
                return true;
            }
            else
                return false;
        }
        public void AddRateToBook(int rate,int bookid)
        {
            var book = db.Books.Where(x => x.Id == bookid).Single();
            db.BooksRates.Add(new BookRate
            {
                Value = (short)rate,
                Date = DateTime.Now,
                Type = RateType.BookRate,
                FkBook = book.Id,
                Book = book
            }) ;
            db.SaveChanges();
        }
        public List<AuthorDTO> GetAuthors(PaginationDTO pagination)
        {
            return db.Authors
                .Include(a => a.Rates)
                .Include(a => a.Books)
                .Skip(pagination.Page * pagination.Count)
                .Take(pagination.Count)
                .ToList().Select(a => new AuthorDTO
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    SecondName = a.SecondName,
                    AvarageRate = (a.Rates.Count() > 0 ? a.Rates.Average(r => r.Value) : 0),
                    RatesCount = a.Rates.Count(),
                    Books = a.Books.Select(b => new SlimBookDTO
                    {
                        Id = b.Id,
                        Title = b.Title
                    }).ToList()
                }).ToList();
        }
        public void PostAuthor(AuthorRequestDTO author)
        {
            Author newauthor = new Author { FirstName = author.FirstName, SecondName = author.SecondName };
            newauthor.Books = db.Books.Where(b => author.BooksId.Contains(b.Id)).ToList();
            db.Authors.Add(newauthor);
            db.SaveChanges();
        }
        public void DeleteAuthor(int id)
        {
            var authortodelete = db.Authors.Include(a => a.Books).Where(a => a.Id == id).Single();
            if (authortodelete != null)
            {
                if (authortodelete.Books.Count > 0)
                {
                    db.Authors.Remove(authortodelete);
                    db.SaveChanges();
                }
            }
        }
        public void AddRateToAuthor(int rate,int authorid)
        {
            var author = db.Authors.Where(a => a.Id == authorid).Single();
            if (author!=null)
            {
                db.AuthorsRates.Add(new AuthorRate
                {
                    Value = (short)rate,
                    Date = DateTime.Now,
                    Type = RateType.AuthorRate,
                    FkAuthor = author.Id,
                    Author = author
                });
                db.SaveChanges();
            }
        }
    }
}
