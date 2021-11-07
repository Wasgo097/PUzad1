using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Model;
using Model.DTO;

namespace RepositoryPattern
{
    public class BooksRepository
    {
        private Database db { get; }
        public BooksRepository(Database db)
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
        public BookDTO PostBook(BookRequestDTO bookdto)
        {
            Book newbook = new Book
            {
                Title = bookdto.Title,
                ReleaseDate = bookdto.ReleaseDate,

            };
            newbook.Authors = db.Authors.Where(a => bookdto.AuthorsIds.Contains(a.Id)).ToList();
            db.Books.Add(newbook);
            db.SaveChanges();
            return new BookDTO
            {
                Id = newbook.Id,
                Authors = newbook.Authors.Select(a => new BookAuthorDTO
                {
                    Id = a.Id,
                    FirstName = a.FirstName,
                    SecondName = a.SecondName

                }).ToList(),
                AvarageRate = 0,
                RatesCount = 0,
                ReleaseDate = newbook.ReleaseDate,
                Title = newbook.Title
            };
        }
        public bool DeleteBook(int index)
        {
            var book = db.Books.Where(x => x.Id == index).Single();
            if (book != null)
            {
                db.Books.Remove(book);
                return true;
            }
            else
                return false;
        }
        public void AddRateToBook(int rate,int bookid)
        {

        }
        public List<BookAuthorDTO> GetAuthors(PaginationDTO pagination)
        {

            return null;
        }
        public void PostAuthor(BookAuthorDTO author)
        {
            Author newauthor = new Author { FirstName = author.FirstName, SecondName = author.SecondName };
            newauthor.Rates = new List<AuthorRate>();
            newauthor.Books = new List<Book>();
            db.Authors.Add(newauthor);
            db.SaveChanges();
        }
    }
}
