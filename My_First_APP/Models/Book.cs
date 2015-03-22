﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace My_First_APP.Models
{
    public class Book
    {
        // ID книги
        public int Id { get; set; }
        // название книги
        public string Login { get; set; }
        // автор книги
        public string Author { get; set; }
        // цена
        public int Price { get; set; }
    }
}