﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using megazlo.Code;
using megazlo.Models;
using System.Configuration;
using System.Web.Configuration;
using System.IO;

namespace megazlo.Controllers {

	[Authorize]
	public class AdminController : Controller {
		string view = "Post";
		ZloContext con = new ZloContext();

		public ActionResult Index() {
			ViewBag.Title = "Админка";
			return View();
		}

		public ActionResult Post(string id) {
			Post pst = new Post() { IsCommentable = true, IsShowInfo = true };
			if (id != null) {
				int ids = int.Parse(id);
				pst = con.Posts.Where(p => p.Id == ids).FirstOrDefault();
			}
			return View(pst);
		}

		[HttpPost]
		public JsonResult Post(string id, string dop) {
			JsonResult rez = new JsonResult();
			Post pst = new Post() { IsCommentable = true, IsShowInfo = true };
			if (id != null) {
				int ids = int.Parse(id);
				pst = con.Posts.Where(p => p.Id == ids).FirstOrDefault();
			}
			rez.Data = RenderPartialViewToString("Post", pst);
			return rez;
		}

		[HttpPost]
		public JsonResult PostSave(Post post) {
			JsonResult rez = new JsonResult();
			if (!ModelState.IsValid) {
				foreach (var item in ModelState.Keys)
					for (int i = 0; i < ModelState[item].Errors.Count; i++)
						rez.Data += ModelState[item].Errors[i].ErrorMessage + "<br />";
				return rez;
			}
			post.Text = Uploader.Parce(post.Text);
			post.WebLink = Uploader.ParceLink(post.Title);
			post.UserId = User.Identity.Name;
			if (post.Id == 0)
				con.Posts.Add(post);
			else
				con.Entry(post).State = System.Data.EntityState.Modified;
			con.SaveChanges();
			if (post.InCatMenu)
				MenuHelper.UpdateCache();
			rez.Data = "Статья успешно сохранена.;" + post.Id;
			return rez;
		}

		[HttpPost]
		public JsonResult PostDelete(int id) {
			JsonResult rez = new JsonResult();
			Post post = con.Posts.Where(p => p.Id == id).FirstOrDefault();
			con.Entry(post).State = System.Data.EntityState.Deleted;
			con.SaveChanges();
			if (post.InCatMenu)
				MenuHelper.UpdateCache();
			rez.Data = "Статья успешно удалена.";
			return rez;
		}

		//public ActionResult DeleteNews(int id) {
		//  Post post = con.Posts.Where(p => p.Id == id).FirstOrDefault();
		//  con.Entry(post).State = System.Data.EntityState.Deleted;
		//  con.SaveChanges();
		//  ViewBag.Title = "Удалено";
		//  if (post.InCatMenu)
		//    MenuHelper.UpdateCache();
		//  return RedirectToAction("Index");
		//}

		public ActionResult AddCategory() {
			ViewBag.Title = "Добавить категорию";
			return View("Category");
		}

		[HttpPost]
		public ActionResult AddCategory(Category cat) {
			ViewBag.Title = "Добавить категорию";
			con.Categorys.Add(cat);
			con.SaveChanges();
			MenuHelper.UpdateCache();
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
			MenuHelper.UpdateCache();
			return RedirectToAction("CategoryList");
		}

		public JsonResult LoadTags(string test) {
			JsonResult rez = new JsonResult();
			Tag[] tg = con.Tags.ToArray();
			string[] arr = new string[tg.Count()];
			for (int i = 0; i < arr.Length; i++)
				arr[i] = tg[i].Title;
			rez.Data = arr;
			return rez;
		}

		public ActionResult ShortUrl(string cod) {
			// TODO: Short ru ССылки
			// TODO: Короткие ru ССылки
			Response.StatusCode = 301;
			Response.StatusDescription = "Moved Permanently";
			Response.AddHeader("Location", "/Home");
			return null;
		}

		public ActionResult TagList() {
			ViewBag.Title = "Список тегов";
			List<Tag> rez = con.Tags.ToList();
			return View(rez);
		}

		public JsonResult DelTag(int id) {
			JsonResult rez = new JsonResult();
			con.Entry(new Tag() { Id = id }).State = System.Data.EntityState.Deleted;
			con.SaveChanges();
			rez.Data = "Тег удален";
			return rez;
		}

		public JsonResult AddTag(string title) {
			JsonResult rez = new JsonResult();
			int cnt = con.Tags.Where(t => t.Title == title).Count();
			if (cnt > 0)
				rez.Data = "Такой тег уже существует";
			else {
				con.Tags.Add(new Tag() { Title = title });
				con.SaveChanges();
				rez.Data = "тег добавлен в базу.";
			}
			return rez;
		}

		public JsonResult AddTag(int id, string title) {
			JsonResult rez = new JsonResult();
			Tag tg = con.Tags.Where(t => t.Id == id).FirstOrDefault();
			if (tg != null) {
				tg.Title = title;
				con.Entry(tg).State = System.Data.EntityState.Modified;
				con.SaveChanges();
				rez.Data = "тег изменен.";
			} else rez.Data = "Такой тег не найден!";
			return rez;
		}

		protected string RenderPartialViewToString(string viewName, object model) {
			if (string.IsNullOrEmpty(viewName))
				return string.Empty;
			ViewData.Model = model;
			using (StringWriter sw = new StringWriter()) {
				ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
				ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
				viewResult.View.Render(viewContext, sw);
				return sw.GetStringBuilder().ToString();
			}
		}

	}
}
