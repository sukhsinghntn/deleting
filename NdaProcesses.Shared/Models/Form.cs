using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;

namespace DynamicFormsApp.Shared.Models
{
    public class Form
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string CreatedBy { get; set; }
        public bool RequireLogin { get; set; } = true;
        public bool NotifyOnResponse { get; set; } = false;
        public string? NotificationEmail { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public bool IsDraft { get; set; } = false;
        public int Version { get; set; } = 1;
        public int? PreviousVersionId { get; set; }
        public List<FormField>? Fields { get; set; }
    }
}
