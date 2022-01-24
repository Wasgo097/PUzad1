using Microsoft.AspNetCore.Mvc;
using Model;
using Model.DTO;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgramowanieUzytkoweIP12.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InitController : ControllerBase
    {
        private Database db { get; }
        private readonly IElasticClient _elasticClient;
        public InitController(Database database, IElasticClient client)
        {
            db = database;
            _elasticClient = client;
        }
        [HttpGet("init")]
        public bool Init()
        {
            //if (_elasticClient.Indices.Exists("authors_index").Exists)
            //{
            //    _elasticClient.Indices.Delete("authors_index");
            //}
            //if (_elasticClient.Indices.Exists("books_index").Exists)
            //{
            //    _elasticClient.Indices.Delete("books_index");
            //}
            db.Books.RemoveRange(db.Books);
            db.Authors.RemoveRange(db.Authors);
            db.AuthorsRates.RemoveRange(db.AuthorsRates);
            db.BooksRates.RemoveRange(db.BooksRates);
            db.SaveChanges();
            var femalenames = "Ada,Adela,Adelajda,Adrianna,Agata";
            var femalenameslist = femalenames.Split(',');
            var malenames = "Adam,Adolf,Adrian,Albert,Aleksander";
            var malenameslist = malenames.Split(',');
            var surnames = "Nowak,Kowal,Wójcik,Kowalczyk,Woźniak";
            var surnameslist = surnames.Split(',');
            var books = "Władca pierścieni,Buszujący w zbożu,Harry Potta,Duma i uprzedzenie,Paragraf 22,Wielki Gatsby,Alicja w Krainie Czarów,Kubuś Puchatek,Anna Karenina,Sto lat samotności";
            var bookslist = books.Split(',');
            List<Author> authorscollection = new List<Author>();
            List<Book> bookscollection = new List<Book>();
            for(int i = 0; i < 5; i++)
            {
                var authordto = new AuthorDTO { FirstName = femalenameslist[i], SecondName = surnameslist[i] };
                var author = new Author { FirstName = authordto.FirstName, SecondName = authordto.SecondName ,CV="CV"+i.ToString()};
                author.Books = new List<Book>();
                author.Rates = new List<AuthorRate>();
                author.Rates.Add(new AuthorRate { Value = 5 });
                db.Authors.Add(author);
                authorscollection.Add(author);
            }
            for(int i = 0; i < 5; i++)
            {
                var authordto = new AuthorDTO { FirstName = malenameslist[i], SecondName = surnameslist[i] };
                var author = new Author { FirstName = authordto.FirstName, SecondName = authordto.SecondName, CV = "CV2" + i.ToString() };
                author.Books = new List<Book>();
                author.Rates = new List<AuthorRate>();
                db.Authors.Add(author);
                authorscollection.Add(author);
            }
            for(int i = 0; i < 10; i++)
            {
                var bookdto = new BookDTO { Title = bookslist[i], ReleaseDate = DateTime.Now };
                var book = new Book { Title = bookdto.Title, ReleaseDate = bookdto.ReleaseDate,Description="Desc"+i.ToString() };
                book.Rates = new List<BookRate>();
                book.Rates.Add(new BookRate { Value=5}) ;
                book.Authors = new List<Author>();
                book.Authors.Add(authorscollection[i]);
                db.Books.Add(book);
                bookscollection.Add(book);
            }
            for(int i = 0; i < 10; i++)
            {
                authorscollection[i].Books.Add(bookscollection[i]);
            }
            db.SaveChanges();
            //foreach(var author in authorscollection)
            //{
            //    var authordto = new AuthorDTO { Id = author.Id, FirstName = author.FirstName, SecondName = author.SecondName,CV=author.CV};
            //    _elasticClient.IndexDocument<AuthorDTO>(authordto);
            //}
            //foreach(var book in bookscollection)
            //{
            //    var bookdto = new BookDTO {Id=book.Id,Title=book.Title,ReleaseDate=book.ReleaseDate,Description=book.Description };
            //    _elasticClient.IndexDocument<BookDTO>(bookdto);
            //}
            return true;
        }
    }
}
