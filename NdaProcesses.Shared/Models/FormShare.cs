namespace DynamicFormsApp.Shared.Models
{
    public class FormShare
    {
        public int Id { get; set; }
        public int FormId { get; set; }
        public string UserName { get; set; } = string.Empty;
    }
}
