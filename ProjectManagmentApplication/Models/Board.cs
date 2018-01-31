﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace ProjectManagementApplication.Models
{
    public class Board
    {
        public int BoardId { get; set; }
        public string Title { get; set; }
        public List<Column> Columns { get; set; }
    }
}