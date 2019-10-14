namespace GoogleSheetsApiV4.Models
{
    public class SheetProperties
    {
        public int SheetId { get; set; }
        public string Title { get; set; }
        public int Index { get; set; }
        public string SheetType { get; set; }
        public GridProperties GridProperties { get; set; }
        public Color TabColor { get; set; }
    }
}
