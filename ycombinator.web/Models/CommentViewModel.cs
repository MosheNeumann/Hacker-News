using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ycombinator.data;

namespace ycombinator.web.Models
{
    public class CommentViewModel
    {
        public List<Comment> Comments { get; set; }

        public User User { get; set; }

        public Article article { get; set; }

        public string Comment { get; set; }
    }
}