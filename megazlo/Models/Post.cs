﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace megazlo.Models {
	public class Post {
		public Post() {
			DatePost = DateTime.Now;
			WebLink = string.Empty;
		}
		[Key]
		public int Id { get; set; }
		[Required]
		[Display(Name = "Заголовок")]
		public string Title { get; set; }
		[Required]
		[Display(Name = "Текст")]
		[DataType(DataType.MultilineText)]
		[MaxLength]
		[AllowHtml]
		public string Text { get; set; }
		[NotMapped]
		public string TextPreview {
			get {
				if (string.IsNullOrEmpty(Text))
					return string.Empty;
				int from = Text.IndexOf("<p>");
				return Text.Substring(from, Text.IndexOf("</p>") - from + 4);
			}
		}
		[MaxLength(200)]
		public string WebLink { get; set; }
		[Display(Name = "Дата создания")]
		public DateTime DatePost { get; set; }
		[Display(Name = "Разрешить комментировать")]
		public bool IsCommentable { get; set; }
		public virtual ICollection<Comment> Comment { get; set; }
		public string UserId { get; set; }
		public virtual User User { get; set; }
		public int? CategoryId { get; set; }
		public virtual Category Category { get; set; }
		[Display(Name = "Категория")]
		[NotMapped]
		public SelectList Cat {
			get {
				using (ZloContext cn = new ZloContext())
					return new SelectList(cn.Categorys.ToList(), "Id", "Title");
			}
		}
		[Display(Name = "В меню категории")]
		public bool InCatMenu { get; set; }
		[Display(Name = "Отображать сведения")]
		public bool IsShowInfo { get; set; }
		[NotMapped]
		public Comment NewComment { get; set; }
	}
}