using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ycombinator.data;

namespace ycombinator.web.Models
{
    public class IndexViewModel
    {
        public List<ArticlePlus> Articles { get; set; }
        public int UserId { get; set; }
    }
}