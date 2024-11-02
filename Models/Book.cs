using System;
using System.Collections.Generic;

namespace Books.Models;

public partial class Book
{
    public int BookId { get; set; }

    public string? Title { get; set; }

    public string? Author { get; set; }

    public int? BookStatus { get; set; }

    public int? CategoryId { get; set; }

    public DateOnly? PublicDate { get; set; }

    public virtual Category? Category { get; set; }
}
