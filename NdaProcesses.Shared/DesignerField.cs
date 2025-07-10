namespace DynamicFormsApp.Shared
{
    public class DesignerField
    {
        public string Key { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public string FieldType { get; set; } = "text";
        public bool IsRequired { get; set; }

        // Dynamic options
        public List<string> OptionItems { get; set; } = new();

        // Grid layout (for grid types)
        public List<string> GridRows { get; set; } = new();
        public List<string> GridColumns { get; set; } = new();

        // Linear scale
        public int ScaleMin { get; set; } = 1;
        public int ScaleMax { get; set; } = 5;

    }
}
