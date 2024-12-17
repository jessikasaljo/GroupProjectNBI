using Domain;

namespace Infrastructure.Database
{
    //public class MockDatabase : IDatabase
    //{
        //public Dictionary<int, Book> BookDBMock;
        //public Dictionary<int, Author> AuthorDBMock;
        //public Dictionary<string, User> UsersDBMock;

        //public MockDatabase()
        //{
        //    UsersDBMock = new Dictionary<string, User>()
        //    {
        //        {"Admin", new User()
        //        {
        //            UserName = "Admin",
        //            Id = 1,
        //            UserPass = "Admin"
        //        } },
        //        {"Dave", new User()
        //        {
        //            UserName = "Dave",
        //            Id = 2,
        //            UserPass = "hunter2"
        //        } }
        //    };

        //    AuthorDBMock = new()
        //    {
        //        {0, new Author() {
        //            Name = "F. Scott Fitzgerald",
        //            Books = new List<int> { 0 } } },
        //        {1, new Author() {
        //            Name = "Harper Lee",
        //            Books = new List<int> { 1 } } },
        //        {2, new Author() {
        //            Name = "George Orwell",
        //            Books = new List<int> { 2 } } },
        //        {3, new Author() {
        //            Name = "J. D. Salinger",
        //            Books = new List<int> { 3 } } },
        //        {4, new Author() {
        //            Name = "J. R. R. Tolkien",
        //            Books = new List<int> { 4 } } }
        //    };

        //    BookDBMock = new()
        //    {
        //        {0, new Book() {
        //            Title = "The Great Gatsby",
        //            Author = "F. Scott Fitzgerald",
        //            Description = "The Great Gatsby is a 1925 novel by American writer F. Scott Fitzgerald. Set in the Jazz Age on Long Island, near New York City, the novel depicts first-person narrator Nick Carraway's interactions with mysterious millionaire Jay Gatsby and Gatsby's obsession to reunite with his former lover, Daisy Buchanan.",
        //            Genre = "Novel",
        //            Date = new DateTime(1925, 4, 10)
        //            } },
        //        {1, new Book() {
        //            Title = "To Kill a Mockingbird",
        //            Author = "Harper Lee",
        //            Description = "To Kill a Mockingbird is a novel by the American author Harper Lee. It was published in 1960 and was instantly successful. In the United States, it is widely read in high schools and middle schools.",
        //            Genre = "Novel",
        //            Date = new DateTime(1960, 7, 11)
        //            } },
        //        {2, new Book() {
        //            Title = "1984",
        //            Author = "George Orwell",
        //            Description = "1984 is a dystopian social science fiction novel by English novelist George Orwell. It was published on 8 June 1949 by Secker & Warburg as Orwell's ninth and final book completed in his lifetime.",
        //            Genre = "Novel",
        //            Date = new DateTime(1949, 6, 8)
        //            } },
        //        {3, new Book() {
        //            Title = "The Catcher in the Rye",
        //            Author = "J. D. Salinger",
        //            Description = "The Catcher in the Rye is a novel by J. D. Salinger, partially published in serial form in 1945–1946 and as a novel in 1951. It was originally intended for adults but is often read by adolescents for its themes of angst, alienation, and as a critique on superficiality in society.",
        //            Genre = "Novel",
        //            Date = new DateTime(1951, 7, 16)
        //            } },
        //        {4, new Book() {
        //            Title = "The Lord of the Rings",
        //            Author = "J. R. R. Tolkien",
        //            Description = "The Lord of the Rings is an epic high-fantasy novel by the English author and scholar J. R. R. Tolkien. Set in Middle-earth, the world at some distant time in the past, the story began as a sequel to Tolkien's 1937 children's book The Hobbit, but eventually developed into a much larger work.",
        //            Genre = "Novel",
        //            Date = new DateTime(1954, 7, 29)
        //        } }
        //    };

        //}

        //public User? LoginUser(string username, string password)
        //{
        //    if (!UsersDBMock.ContainsKey(username))
        //    {
        //        return null;
        //    }
        //    var user = UsersDBMock[username];
        //    if (user.UserPass == password)
        //    {
        //        return user;
        //    }
        //    return null;
        //}

        //public Task<bool> Add<T>(string table, T item)
        //{
        //    if (table == "Book")
        //    {
        //        return Task.FromResult(AddBook(item as Book));
        //    }
        //    else if (table == "Author")
        //    {
        //        return Task.FromResult(AddAuthor(item as Author));
        //    }
        //    else if (table == "User")
        //    {
        //        return Task.FromResult<bool>(AddUser(item as User));
        //    }
        //    return Task.FromResult(true);
        //}
        //private bool AddUser(User user)
        //{
        //    UsersDBMock.Add(user.UserName, user);
        //    return true;
        //}
        //private bool AddBook(Book book)
        //{
        //    BookDBMock.Add(BookDBMock.Count, book);
        //    return true;
        //}
        //private bool AddAuthor(Author author)
        //{
        //    if (author == null)
        //    {
        //        return false;
        //    }
        //    AuthorDBMock.Add(AuthorDBMock.Count, author);
        //    return true;
        //}

        //public Task<bool> DeleteById(string table, int id)
        //{
        //    if (table == "Book")
        //    {
        //        return Task.FromResult(DeleteBookById(id));
        //    }
        //    else if (table == "Author")
        //    {
        //        return Task.FromResult(DeleteAuthorById(id));
        //    }
        //    return Task.FromResult(false);
        //}

        //private bool DeleteAuthorById(int id)
        //{
        //    if (AuthorDBMock.ContainsKey(id))
        //    {
        //        AuthorDBMock.Remove(id);
        //        return true;
        //    }
        //    return false;
        //}

        //private bool DeleteBookById(int id)
        //{
        //    if (BookDBMock.ContainsKey(id))
        //    {
        //        BookDBMock.Remove(id);
        //        return true;
        //    }
        //    return false;
        //}

        //public Task<Dictionary<int, T>> GetAll<T>(string table)
        //{
        //    if (table == "Book" && typeof(T) == typeof(Book))
        //    {
        //        return Task.FromResult(BookDBMock as Dictionary<int, T>);
        //    }
        //    else if (table == "Author" && typeof(T) == typeof(Author))
        //    {
        //        return Task.FromResult(AuthorDBMock as Dictionary<int, T>);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Error");
        //    }
        //}


        //public Task<T> GetById<T>(string table, int id)
        //{
        //    if (table == "Book" && typeof(T) == typeof(Book))
        //    {
        //        return Task.FromResult((T)(object)BookDBMock[id]);
        //    }
        //    else if (table == "Author" && typeof(T) == typeof(Author))
        //    {
        //        return Task.FromResult((T)(object)AuthorDBMock[id]);
        //    }
        //    else
        //    {
        //        throw new ArgumentException("Error");
        //    }
        //}

        //public Task<bool> UpdateById<T>(string table, int id, T book)
        //{
        //    if (table == "Book" && typeof(T) == typeof(Book) && BookDBMock.ContainsKey(id))
        //    {
        //        BookDBMock[id] = book as Book;
        //        return Task.FromResult(true);
        //    }
        //    else if (table == "Author" && typeof(T) == typeof(Author) && AuthorDBMock.ContainsKey(id))
        //    {
        //        AuthorDBMock[id] = book as Author;
        //        return Task.FromResult(true);
        //    }
        //    return Task.FromResult(false);
        //}
    //}
}
