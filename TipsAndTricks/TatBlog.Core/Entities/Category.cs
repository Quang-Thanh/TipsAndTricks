using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TatBlog.Core.Contracts;

namespace TatBlog.Core.Entities
{
    public class Category : IEntity
    {
        //mã chuyên mục
        public int Id { get; set; }

        //tên chuyên mục, chủ đề
        public string Name { get; set; }

        //Tên định dạng để tạo URL
        public string UrlSlug { get; set; }

        //mô tả chuyên mục
        public string Description { get; set; }

        //Đánh giá chuyên mục được hiện thị trên menu
        public bool ShowOnMenu { get; set; }

        //Danh sách các bài viết thuộc chuyên mục
        public IList<Post> Posts { get; set; }
    }
}
