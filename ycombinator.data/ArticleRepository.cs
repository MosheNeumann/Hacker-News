using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ycombinator.data
{
public    class ArticleRepository
    {
        private string _connectionString;

        public ArticleRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<ArticlePlus> GetAllArticls()
        {
            using (DBContextDataContext context = new DBContextDataContext(_connectionString))
            {
                var LoadOptions = new DataLoadOptions();
                LoadOptions.LoadWith<Article>(A => A.User);
                LoadOptions.LoadWith<Article>(A => A.Likes);
                LoadOptions.LoadWith<Article>(A => A.Comments);
                context.LoadOptions = LoadOptions;

                IEnumerable<Article> articles = context.Articles;

                List<ArticlePlus> ArticleWithVoteCount = GetArticlesWithVoteCount(context.Articles.ToList());
                return ArticleWithVoteCount;


            }
          
        }
        public List<ArticlePlus>GetArticlesWithVoteCount(List<Article> Articles) 
        {
           // List<Article> Articles = GetAllArticls();
            List<ArticlePlus> ArticlesPlus = new List<ArticlePlus>();
            for (int x = 0; x < Articles.Count; x++)
            {
                ArticlePlus plus = new ArticlePlus();
                plus.Id = Articles[x].Id;
                plus.Title = Articles[x].Title;
                plus.URL = Articles[x].URL;
                plus.UploadedBy = Articles[x].UploadedBy;
                plus.User = Articles[x].User;
                plus.Likes = Articles[x].Likes;
                plus.VoteCountUp = Articles[x].Likes.Where(A =>A.ThumbsUp==ThumbsUpEnum.True).Count();
                plus.VoteCountDown= Articles[x].Likes.Where(A =>A.ThumbsUp== ThumbsUpEnum.False).Count();
                ArticlesPlus.Add(plus);
            }
            ArticlesPlus.OrderBy(A => A.VoteCountUp);
            return ArticlesPlus;
        }
        public void AddArticle(Article article)
        {
            using (DBContextDataContext context = new DBContextDataContext(_connectionString))
            {
                context.Articles.InsertOnSubmit(article);
                context.SubmitChanges();

            }

        }
        public void InsertLike(Like like)
        {
            using (DBContextDataContext context = new DBContextDataContext(_connectionString))
            {
                context.Likes.InsertOnSubmit(like);
                context.SubmitChanges();
            }

        }
        public List<ArticlePlus>GetArticlesForUser(int id)
        {
            List<ArticlePlus> ArticlesForUser = GetAllArticls();
            return ArticlesForUser.Where(A => A.User.Id == id).ToList();
        }

        public List<Comment> Comments(int id)
        {
            using (DBContextDataContext context = new DBContextDataContext(_connectionString))
            {
                var LoadOptions = new DataLoadOptions();
                LoadOptions.LoadWith<Comment>(C => C.User);
                LoadOptions.LoadWith<Comment>(C => C.Article);
                context.LoadOptions = LoadOptions;
                return context.Comments.Where(C => C.ArticleId == id).ToList();
            }
        }

        public void AddComment(Comment comment)
        {
            using (DBContextDataContext context = new DBContextDataContext(_connectionString))
            {
                context.Comments.InsertOnSubmit(comment);
                context.SubmitChanges();
            }
        }

        public Article GetArticle(int id)
        {
            using(DBContextDataContext context = new DBContextDataContext(_connectionString))
            {
                return context.Articles.FirstOrDefault(A => A.Id == id);
            }
        }


    }
}
