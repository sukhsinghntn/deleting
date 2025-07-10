using System;
using System.Collections.Generic;

namespace DynamicFormsApp.Shared.Models
{
    public class EmailModel
    {
        public string? Body { get; set; }
        public string? Link { get; set; }
        public int? Id { get; set; }
        public string? UserName { get; set; }

        public string? TOUserName { get; set; }
        public List<string>? Emails { get; set; }

        // --- now supports multiple attachments ---
        /// <summary>
        /// A list of file attachments (screenshots, logs, etc.).
        /// </summary>
        public List<EmailAttachment>? Attachments { get; set; }
    }
}
