using System;

namespace Bermuda.Core.Database.Entity
{
    public class EntityBase
    {
        public virtual long Id { get; set; }
        public virtual Guid Guid { get; set; }
        public virtual StatusType StatusType { get; set; }

        public EntityBase()
        {
            this.Guid = Guid.NewGuid();
            this.StatusType = StatusType.Active;
        }
    }
}
