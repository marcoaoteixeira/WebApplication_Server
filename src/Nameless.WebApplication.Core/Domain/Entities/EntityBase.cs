﻿namespace Nameless.WebApplication.Domain.Entities {
    public abstract class EntityBase {

        #region Public Virtual Properties

        public virtual Guid ID { get; set; }
        public virtual DateTime CreationDate { get; set; }
        public virtual DateTime ModificationDate { get; set; }

        #endregion
    }
}
