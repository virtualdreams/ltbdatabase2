﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ltbdb.Models
{
	public class CategoryModel
	{
		public int Id { get; set; }
		
		[Required(ErrorMessage="Bitte gib einen Namen ein.")]
		public string Name { get; set; }
	}
}