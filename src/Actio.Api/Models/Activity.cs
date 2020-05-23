using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Api.Models
{
    public class Activity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }

        public override string ToString()
        {
            return $"Id - {this.Id}, Name - {this.Name}, Category - {this.Category}, Description - {this.Description}, UserId - {this.UserId}, CreatedAt - {this.CreatedAt}";
        }
    }
}
