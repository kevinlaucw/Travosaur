using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Travosaur.Models
{
    public class Subscriber
    {
        public int SubscriberID { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}