﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using megazlo.Code;
using megazlo.Models;

namespace megazlo.Controllers {

	[Authorize]
	public class AdminController : Controller {
		string view = "Post";
		ZloContext con = new ZloContext();

		public ActionResult Index() {
			ViewBag.Title = "Админка";
			return View();
		}

		public ActionResult AddNews() {
			ViewBag.Title = "Добавление статей";
			ViewBag.ButtonOk = "Создать";
			return View(view, new Post());
		}

		[HttpPost]
		public ActionResult AddNews(Post post) {
			if (!ModelState.IsValid)
				return RedirectToAction("Post", "Home", new { id = post.WebLink });
			ViewBag.Title = "Добавление статей";
			ViewBag.ButtonOk = "Создать";
			post.Text = PostXml.Parce(post.Text);
			post.WebLink = PostXml.ParceLink(post.Title);
			post.UserId = User.Identity.Name;
			con.Posts.Add(post);
			con.SaveChanges();
			return RedirectToAction("Post", "Home", new { id = post.WebLink });
		}

		public ActionResult EditNews(int id) {
			Post pst = con.Posts.Where(p => p.Id == id).First();
			ViewBag.ButtonOk = "Изменить";
			ViewBag.Title = "Редактирование статьи: " + pst.Title;
			return View(view, pst);
		}

		[HttpPost]
		public ActionResult EditNews(Post post) {
			if (!ModelState.IsValid)
				return View(view, post);
			post.Text = PostXml.Parce(post.Text);
			post.WebLink = PostXml.ParceLink(post.Title);
			con.Entry(post).State = System.Data.EntityState.Modified;
			con.SaveChanges();
			ViewBag.ButtonOk = "Изменить";
			ViewBag.Title = "Редактирование статьи: " + post.Title;
			return RedirectToAction("Post", "Home", new { id = post.WebLink });
		}

		public ActionResult DeleteNews(int id) {
			Post post = new Post() { Id = id };
			con.Entry(post).State = System.Data.EntityState.Deleted;
			con.SaveChanges();
			ViewBag.Title = "Удалено";
			return RedirectToAction("Index");
		}

		public ActionResult AddCategory() {
			ViewBag.Title = "Добавить категорию";
			return View("Category");
		}

		[HttpPost]
		public ActionResult AddCategory(Category cat) {
			ViewBag.Title = "Добавить категорию";
			con.Categorys.Add(cat);
			con.SaveChanges();
			return RedirectToAction("CategoryList");
		}

		public ActionResult EditCategory(int id) {
			Category cat = con.Categorys.Where(p => p.Id == id).First();
			ViewBag.ButtonOk = "Изменить";
			ViewBag.Title = "Редактирование категории: " + cat.Title;
			return View("Category", cat);
		}

		public ActionResult CategoryList() {
			ViewBag.Title = "Категории";
			List<Category> mod = con.Categorys.OrderBy(c => c.Por).ToList();
			return View(mod);
		}

		public ActionResult DeleteCat(int id) {
			Category cat = new Category() { Id = id };
			con.Entry(cat).State = System.Data.EntityState.Deleted;//.Categorys.Remove(cat);
			con.SaveChanges();
			return RedirectToAction("CategoryList");
		}

	}
}
