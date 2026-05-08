using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;

namespace GroveStart.Model
{
    public class Reason
    {
        [Key]
        public int Id {get;private set;}
        public string Title {get;private set;}

        public string Description{get;private set;}

        public Reason(string title, string description)
        {
            Title = title;
            Description = description;
        }

    }
}