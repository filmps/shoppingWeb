using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineWeb.Models  
{

    public class Feedback
    {
        [Key]
        public int Id { get; set; }
        public required string Title { get; set; }

        public required string Content { get; set; }
        public required string UserEmail { get; set; }

    }
}
