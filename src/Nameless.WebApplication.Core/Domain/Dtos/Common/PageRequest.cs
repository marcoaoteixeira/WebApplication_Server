namespace Nameless.WebApplication.Domain.Dtos.Common {

    public sealed class PageRequest {

        #region Public Properties

        public string? SearchTerm { get; set; }
        public string? OrderBy { get; set; }
        public int Index { get; set; }
        public int Size { get; set; }

        #endregion
    }
}
