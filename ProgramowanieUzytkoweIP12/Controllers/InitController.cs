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
            if (_elasticClient.Indices.Exists("authors_index").Exists)
            {
                _elasticClient.Indices.Delete("authors_index");
            }
            if (_elasticClient.Indices.Exists("books_index").Exists)
            {
                _elasticClient.Indices.Delete("books_index");
            }
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
            List<AuthorDTO> authorsdto = new List<AuthorDTO>();
            List<BookDTO> booksdto = new List<BookDTO>();
            for(int i = 0; i < 5; i++)
            {
                var author = new AuthorDTO { FirstName = femalenameslist[i], SecondName = surnameslist[i] };
                authorsdto.Add(author);
                db.Authors.Add(new Author { FirstName = author.FirstName, SecondName = author.SecondName });
            }
            for(int i = 0; i < 5; i++)
            {
                var author = new AuthorDTO { FirstName = malenameslist[i], SecondName = surnameslist[i] };
                authorsdto.Add(author);
                db.Authors.Add(new Author { FirstName = author.FirstName, SecondName = author.SecondName });
            }
            for(int i = 0; i < 10; i++)
            {
                var book = new BookDTO { Title = bookslist[i], ReleaseDate = DateTime.Now };
                booksdto.Add(book);
                db.Books.Add(new Book { Title=book.Title,ReleaseDate=book.ReleaseDate });
            }
            db.SaveChanges();
            foreach(var author in authorsdto)
            {
                _elasticClient.IndexDocument<AuthorDTO>(author);
            }
            foreach(var book in booksdto)
            {
                _elasticClient.IndexDocument<BookDTO>(book);
            }
            return true;
        }
    }
}
