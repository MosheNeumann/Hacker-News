using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using ycombinator.data;
using ycombinator.web.Models;



namespace ycombinator.web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var Repo = new ArticleRepository(Properties.Settings.Default.ConnectionString);
            var userrepo = new UserRepository(Properties.Settings.Default.ConnectionString);
            var IndexViewModel = new IndexViewModel();
            IndexViewModel.Articles = Repo.GetAllArticls();

            if (User.Identity.IsAuthenticated)
            {
                IndexViewModel.UserId = userrepo.GetUserByEmail(User.Identity.Name).Id;
            }

            return View(IndexViewModel);
        }

        [Route("UploadedByUser/{userId}")]
        public ActionResult ArticlesByUser(int userId)
        {
           
            var Repo = new ArticleRepository(Properties.Settings.Default.ConnectionString);
            var userrepo = new UserRepository(Properties.Settings.Default.ConnectionString);

            var IndexViewModel = new IndexViewModel();
            IndexViewModel.Articles = Repo.GetArticlesForUser(userId);

            if (User.Identity.IsAuthenticated)
            {
                IndexViewModel.UserId = userrepo.GetUserByEmail(User.Identity.Name).Id;
            }

            return View(IndexViewModel);
        }

        [Route("Register")]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(User user, string Password)
        {
            var userRepo = new UserRepository(Properties.Settings.Default.ConnectionString);
            user.PasswordSalt = PasswordHelper.GenerateSalt();
            user.PasswordHash = PasswordHelper.HashPassword(Password, user.PasswordSalt);

            userRepo.AddUser(user);
            FormsAuthentication.SetAuthCookie(user.Email, true);

            return Redirect("/");
        }

        public ActionResult Login()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var userRepo = new UserRepository(Properties.Settings.Default.ConnectionString);
            User user = userRepo.GetUser(email, password);
            if (user == null)
            {
                return Redirect("/Home/Login");
            }
            FormsAuthentication.SetAuthCookie(email, true);
          
            if((Session["CommentForUserLogin"] != null))
            {
                return Redirect($"/Home/Comments?id={(int)Session["articleId"]}");
            }
            return Redirect("/");
        }
        [Authorize]
        public ActionResult UploadArticle()
        {
            if (!(User.Identity.IsAuthenticated))
            {
                return Redirect("/");

            }

            return View();
        }
        [Authorize]
        [HttpPost]
        public ActionResult UploadArticle(Article article)
        {
            if (!(User.Identity.IsAuthenticated))
            {
                return Redirect("/");

            }
            var userRepo = new UserRepository(Properties.Settings.Default.ConnectionString);
            article.UploadedBy= userRepo.GetUserByEmail(User.Identity.Name).Id;
            var ArticleRepo = new ArticleRepository(Properties.Settings.Default.ConnectionString);
            ArticleRepo.AddArticle(article);
            return Redirect("/");
        }

        public ActionResult Comments (int id)
        {
            var ArticleRepo = new ArticleRepository(Properties.Settings.Default.ConnectionString);
            var userrepo = new UserRepository(Properties.Settings.Default.ConnectionString);

            var VM = new CommentViewModel();
            VM.Comments = ArticleRepo.Comments(id);
            VM.article = ArticleRepo.GetArticle(id);
            if (User.Identity.IsAuthenticated)
            {
                VM.User = userrepo.GetUserByEmail(User.Identity.Name);
            }
            if (Session["CommentForUserLogin"] != null)
            {
                VM.Comment = (string)Session["CommentForUserLogin"];
            }
           
            return View(VM);
        }
        [HttpPost]
        public ActionResult Comments(string comment,int ArticleId)
        {
            if (!(User.Identity.IsAuthenticated))
            {
                Session["CommentForUserLogin"] = comment;
                Session["articleId"] = ArticleId;
                return Redirect("/Home/Login");
            }
            Session["CommentForUserLogin"] = null;
            var ArticleRepo = new ArticleRepository(Properties.Settings.Default.ConnectionString);
            var userrepo = new UserRepository(Properties.Settings.Default.ConnectionString);
            Comment C = new Comment
            {
                Comment1 = comment,
                CommentedBy = userrepo.GetUserByEmail(User.Identity.Name).Id,
                ArticleId = ArticleId,
                Date = DateTime.Now

            };
            ArticleRepo.AddComment(C);

            return Redirect($"/Home/Comments?id={ArticleId}");


        }
        public ActionResult LogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return Redirect("/");
        }
    }
}