using System;

namespace DynamicFormsApp.Shared.Models
{
    public class FormField
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string Key { get; set; }      // SQL column name
        public string Label { get; set; }    // UI label
        public string FieldType { get; set; }// e.g., "text","number","dropdown", etc.
        public bool IsRequired { get; set; }

        // Optional: for dropdowns, checkboxes, grids, etc.
        public string? OptionsJson { get; set; }

        // Optional: for layout (future grid fields support)
        public int? Row { get; set; }
        public int? Column { get; set; }
    }
}
