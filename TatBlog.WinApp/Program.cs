// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");
using TatBlog.Data.Contexts;
using TatBlog.Data.Seeders;

var context = new BlogDbContext();

var posts = context.Posts
    .Where(p => p.Published)
    .OrderBy(p => p.Title)
    .Select(p => new
    {
        Id = p.Id,
        Title = p.Title,
        ViewCout = p.ViewCout,
        PostedDate = p.PostedDate,
        Author = p.Author.FullName,
        Category = p.Category.Name
    })
    .ToList();



var seeder = new DataSeeder(context);

seeder.Initialize();

var authors = context.Authors.ToList();

Console.WriteLine("{0, -4}{1, -30}{2, -30}{3, 12}",
    "ID", "Full Name", "Email", "Joined Date");

//foreach (var author in authors)
//{
//    Console.WriteLine("{0, -4}{1, -30}{2, -30}{3, 12:MM/dd/yyyy}", author.Id, author.FullName, author.Email, author.JoineDate);
//}

foreach (var post in posts)
{
    Console.WriteLine("ID       : {0}", post.Id);
    Console.WriteLine("Title    : {0}", post.Title);
    Console.WriteLine("View     : {0}", post.ViewCout);
    Console.WriteLine("Date     : {0:MM/dd/yyyy}", post.PostedDate);
    Console.WriteLine("Author   : {0}", post.Author);
    Console.WriteLine("Category : {0}", post.Category);
    Console.WriteLine("".PadRight(80, '-'));
}
