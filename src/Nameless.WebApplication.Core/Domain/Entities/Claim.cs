namespace Nameless.WebApplication.Domain.Entities {

    public class Claim : EntityBase {

        #region Public Virtual Properties

        public virtual string Name { get; set; } = null!;
        public virtual string Value { get; set; } = null!;
        public virtual User Owner { get; set; } = null!;

        #endregion
    }
}
