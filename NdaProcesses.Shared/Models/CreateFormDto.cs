using System.Collections.Generic;

namespace DynamicFormsApp.Shared.Models
{
    public class CreateFormDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<CreateFieldDto> Fields { get; set; }
        public bool RequireLogin { get; set; } = true;
        public bool NotifyOnResponse { get; set; } = false;
        public string? NotificationEmail { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
        public bool IsDraft { get; set; } = false;
    }
}
