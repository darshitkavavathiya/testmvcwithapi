﻿using System.Collections.Generic;

namespace testmvc.Models
{
    public class UserEmailOptions
    {

        public List<string> ToEmails { get; set; }   
        public string Subject { get; set; } 
        public string Body { get; set; }    

    }
}
