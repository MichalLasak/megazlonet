﻿using System;
using System.Web.Security;

namespace megazlo.Models {
	public interface IMembershipService {
		int MinPasswordLength { get; }

		bool ValidateUser(string userName, string password);
		MembershipCreateStatus CreateUser(string userName, string password, string email);
		bool ChangePassword(string userName, string oldPassword, string newPassword);
	}

	public interface IFormsAuthenticationService {
		void SignIn(string userName, bool createPersistentCookie);
		void SignOut();
	}

	public class FormsAuthenticationService : IFormsAuthenticationService {
		public void SignIn(string userName, bool createPersistentCookie) {
			if (String.IsNullOrEmpty(userName))
				throw new ArgumentException("Value cannot be null or empty.", "userName");
			FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
		}

		public void SignOut() {
			FormsAuthentication.SignOut();
		}
	}

	public class MZMembershipService : IMembershipService {
		int maipas = 6;
		#region IMembershipService Members
		public int MinPasswordLength {
			get { return maipas; }
		}

		public bool ValidateUser(string userName, string password) {
			return DAO.AutoriseUser(new User() { NickName = userName, PassWord = password });
		}

		public MembershipCreateStatus CreateUser(string userName, string password, string email) {
			throw new NotImplementedException();
		}

		public bool ChangePassword(string userName, string oldPassword, string newPassword) {
			throw new NotImplementedException();
		}
		#endregion
	}
}