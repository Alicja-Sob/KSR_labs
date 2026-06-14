using Library.WebApi.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LibraryController : ControllerBase
{
    private readonly IPublishEndpoint _publishEndpoint;

    // Nowoczesna inicjalizacja kolekcji (C# 12)
    private readonly List<LibraryResource> _libraryBooks =
    [
        new() { Id = 1, Book = new() { Title = "Miecz Przeznaczenia", Author = "Andrzej Sapkowski", ISBN = "9788375780642", ReleaseDate = new DateTime(2014, 1, 1) } },
        new() { Id = 2, Book = new() { Title = "Malowany Czlowiek. Ksiega 1", Author = "Peter V. Brett", ISBN = "9788375781111", ReleaseDate = new DateTime(2008, 11, 28) } },
        new() { Id = 10, Book = new() { Title = "Mikolajek", Author = "Rene Goscinny, Jean-Jacques Sempe", ISBN = "9788375780642", ReleaseDate = new DateTime(2014, 9, 13) } },
        new() { Id = 3, Book = new() { Title = "Droga Cienia", Author = "Brent Weeks", ISBN = "9788375780281", ReleaseDate = new DateTime(2017, 5, 31) } },
        new() { Id = 4, Book = new() { Title = "Praktyczny przewodnik. USA", Author = "Monika Gruszczynska", ISBN = "9788555780621", ReleaseDate = new DateTime(2018, 2, 28) } },
        new() { Id = 5, Book = new() { Title = "Mikolajek", Author = "Rene Goscinny, Jean-Jacques Sempe", ISBN = "9788375780642", ReleaseDate = new DateTime(2014, 9, 13) } },
        new() { Id = 6, Book = new() { Title = "Malowany Czlowiek. Ksiega 2", Author = "Peter V. Brett", ISBN = "9788375781221", ReleaseDate = new DateTime(2009, 1, 28) } }
    ];

    public LibraryController(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet("rented")]
    public IEnumerable<LibraryResource> GetRented()
    {
        return _libraryBooks;
    }

    [HttpGet("rent/{id}")]
    public async Task<ActionResult> Rent(int id)
    {
        var rentedBook = _libraryBooks.SingleOrDefault(r => r.Id == id);

        if (rentedBook == null)
        {
            return NotFound($"Book with id: {id} does not exist");
        }

        // MassTransit œwietnie radzi sobie z mapowaniem typów anonimowych na obiekty klas
        await _publishEndpoint.Publish<BookRented>(new { Id = id, Title = rentedBook.Book.Title });
        return Ok($"Wypo¿yczono: {rentedBook.Book.Title}");
    }
}