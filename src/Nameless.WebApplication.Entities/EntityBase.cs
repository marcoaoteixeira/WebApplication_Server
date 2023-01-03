namespace Nameless.WebApplication.Entities {
    public abstract class EntityBase {

        #region Public Properties

        public Guid ID { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }

        #endregion
    }
}
