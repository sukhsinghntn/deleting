using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicFormsApp.Shared.Models
{
    /// <summary>
    /// Represents a single file attachment for an email.
    /// </summary>
    public class EmailAttachment
    {
        /// <summary>
        /// Filename (including extension) of the attached file.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Raw file bytes.
        /// </summary>
        public byte[] Content { get; set; } = Array.Empty<byte>();
    }
}
