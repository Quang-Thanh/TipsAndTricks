using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Entities;
using TatBlog.Data.Contexts;

namespace TatBlog.Data.Seeders
{
    public class DataSeeder : IDataSeeder
    {
        private readonly BlogDbContext _dbContext;

        public DataSeeder(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Initialize()
        {
            _dbContext.Database.EnsureCreated();

            if (_dbContext.Posts.Any()) return;

            var authors = AddAuthors();
            var categories = AddCategories();
            var tags = AddTags();
            var posts = AddPosts(authors, categories, tags);
        }

        private IList<Author> AddAuthors() 
        { 
            var authors = new List<Author>()
            {
                new()
                {
                    FullName = "Jason Mouth",
                    UrlSlug = "jason-mouth",
                    Email = "json@gmail.com",
                    JoineDate = new DateTime(2022, 10, 21)
                },
                new()
                {
                    FullName = "Jessica Wonder",
                    UrlSlug = "jessica-wonder",
                    Email = "jessica665@motip.com",
                    JoineDate = new DateTime(2020, 4, 19)
                },
                new()
                {
                    FullName = "Binh Gon",
                    UrlSlug = "binh-gon",
                    Email = "gudboibg@gmail.com",
                    JoineDate = new DateTime(2021, 4, 22)
                },
                new()
                {
                    FullName = "Dat G",
                    UrlSlug = "dat-g",
                    Email = "datg22@gmail.com",
                    JoineDate = new DateTime(2020, 10, 10)
                },
                new()
                {
                    FullName = "Le Bao",
                    UrlSlug = "le-bao",
                    Email = "lebaoentertainment@gmail.com",
                    JoineDate = new DateTime(2020, 5, 23)
                },
                new()
                {
                    FullName = "Huan Hoa Hong",
                    UrlSlug = "huan-hoa-hong",
                    Email = "huanhoahongchinngon@gmail.com",
                    JoineDate = new DateTime(2020, 4, 3)
                }
            };
            _dbContext.Authors.AddRange(authors);
            _dbContext.SaveChanges();

            return authors;
        }

        private IList<Category> AddCategories() 
        {
            var categories = new List<Category>()
            {
                new() {Name = ".NET Core", Description = ".NET Core", UrlSlug = "net core", ShowOnMenu = true},
                new() {Name = "Architecture", Description = "Architecture", UrlSlug = "architecture", ShowOnMenu = true},
                new() {Name = "Messaging", Description = "Messaging", UrlSlug = "messaging", ShowOnMenu = true},
                new() {Name = "OOP", Description = "Object-Oriented Program", UrlSlug = "oop", ShowOnMenu = true},
                new() {Name = "Design Patterns", Description = "Design Patterns", UrlSlug = "design pattens", ShowOnMenu = true},
                new() {Name = "HTML", Description = "HTML", UrlSlug = "html", ShowOnMenu = true},
                new() {Name = "CSS", Description = "CSS", UrlSlug = "css", ShowOnMenu = true},
                new() {Name = "Java", Description = "Java", UrlSlug = "java", ShowOnMenu = true},
                new() {Name = "PHP", Description = "PHP", UrlSlug = "php", ShowOnMenu = true},
                new() {Name = "C Sharp", Description = "C-Sharp", UrlSlug = "csharp", ShowOnMenu = true},
                new() {Name = "Kinh Di", Description = "Kinh-Di", UrlSlug = "kinhdi", ShowOnMenu = true},
                new() {Name = "Cuoi", Description = "Cuoi", UrlSlug = "cuoi", ShowOnMenu = true},
                new() {Name = "Tinh Cam", Description = "Tinh-Cam", UrlSlug = "tinhcam", ShowOnMenu = true},
                new() {Name = "Doi Thuong", Description = "Doi-Thuong", UrlSlug = "doithuong", ShowOnMenu = true}

            };
            _dbContext.AddRange(categories);
            _dbContext.SaveChanges();

            return categories;
        }

        private IList<Tag> AddTags() 
        {
            var tags = new List<Tag>()
            {
                new() {Name = "Google", Description = "Google applications", UrlSlug = "google"},
                new() {Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "ASP.NET MVC"},
                new() {Name = "Razor Page", Description = "Razor Page", UrlSlug = "razor page"},
                new() {Name = "Blazor", Description = "Blazor", UrlSlug = "blazor"},
                new() {Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "deep learning"},
                new() {Name = "Neural Network", Description = "Neural Network", UrlSlug = "neural network"},
                new() {Name = "Ruby", Description = "Ruby", UrlSlug = "ruby"},
                new() {Name = "Workpress", Description = "Workpress", UrlSlug = "workpress"},
                new() {Name = "Google", Description = "Google applications", UrlSlug = "google"},
                new() {Name = "Blazor", Description = "Blazor", UrlSlug = "blazor"},
                new() {Name = "ASP.NET MVC", Description = "ASP.NET MVC", UrlSlug = "ASP.NET MVC"},
                new() {Name = "Workpress", Description = "Workpress", UrlSlug = "workpress"},
                new() {Name = "Deep Learning", Description = "Deep Learning", UrlSlug = "deep learning"},
                new() {Name = "Neural Network", Description = "Neural Network", UrlSlug = "neural network"},
                new() {Name = "Blazor", Description = "Blazor", UrlSlug = "blazor"}
            };

            _dbContext.AddRange(tags);
            _dbContext.SaveChanges();

            return tags;
        }

        private IList<Post> AddPosts(
            IList<Author> authors,
            IList<Category> categories, 
            IList<Tag> tags)
        {
            var posts = new List<Post>()
            {
                new()
                {
                    Title = "ASP.NET Core Diagnostic Scenarios",
                    ShortDescription = "David and friends has a great repos",
                    Description = "Here's a few  great DON'T and DO example",
                    Meta = "David and friends has a great repository filled",
                    UrlSlug = "aspnet-core-diagnostic-scenarios",
                    Published = true,
                    PostedDate = new DateTime(2021, 9, 30, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCout = 10,
                    Author = authors[0],
                    Category = categories[0],
                    Tags = new List<Tag>()
                    {
                        tags[0]
                    }
                },

                new()
                {
                    Title = "Apple",
                    ShortDescription = "Thương hiệu điện tử nổi tiếng được các bạn trẻ ưa chuộng",
                    Description = "Có nhiều tính năng nổi trội hơn các thương hiệu khác",
                    Meta = "Tự hào khi đứng số 1 trên thị trường hiện nay",
                    UrlSlug = "Apple",
                    Published = true,
                    PostedDate = new DateTime(2009, 9, 10, 10, 20, 0),
                    ModifiedDate = null,
                    ViewCout = 20,
                    Author = authors[1],
                    Category = categories[1],
                    Tags = new List<Tag>()
                    {
                        tags[1]
                    }
                },

            };
            _dbContext.AddRange(posts);
            _dbContext.SaveChanges();

            return posts;
        }
    }
}
