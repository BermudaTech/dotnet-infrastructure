using System;

namespace Bermuda.Core.Database.Entity
{
    public class EntityBaseWithLog<PKey> : EntityBase<PKey>
    {
        public virtual string InsertedUser { get; set; }
        public virtual string InsertedIp { get; set; }
        public virtual DateTime InsertedDate { get; set; }
        public virtual string UpdatedUser { get; set; }
        public virtual string UpdatedIp { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }

        public EntityBaseWithLog()
        {

        }
    }
}
