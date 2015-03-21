using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackOverflow.Domain.Entities;

namespace StackOverflow.Web.Models
{
    public class CommentListModel
    {
        public DateTime CreationDate { get; set; }
        public string Description { get; set; }
        public virtual string OwnerName { get; set; }
    }
}