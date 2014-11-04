using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class LoginModel
	{
		[Required(ErrorMessage="Bitte gib einen Benutzername ein.")]
		public string Username { get; set; }

		[Required(ErrorMessage="Bitte gib ein Passwort ein.")]
		public string Password { get; set; }
	}
}