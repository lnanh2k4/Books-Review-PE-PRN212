using System;
using System.Collections.Generic;

namespace Books.Models;

public partial class Category
{
    public int CatId { get; set; }

    public string? CatName { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
