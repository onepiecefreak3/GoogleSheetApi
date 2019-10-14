namespace GoogleSheetsV4.Models
{
    class ParsedOptions
    {
        public string SheetId { get; }

        public string ClientId { get; }

        public string ClientSecret { get; }

        public ParsedOptions(string sheetId, string clientId, string clientSecret)
        {
            SheetId = sheetId;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }
    }
}
