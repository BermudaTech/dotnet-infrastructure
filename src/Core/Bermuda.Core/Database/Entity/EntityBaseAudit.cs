using System;

namespace Bermuda.Core.Database.Entity
{
    public class EntityBaseAudit<PKey> : EntityBase<PKey>
    {
        public virtual Guid TrackId { get; set; }
        public virtual StateType State { get; set; }
        public virtual string InsertedUser { get; set; }
        public virtual string InsertedIp { get; set; }
        public virtual DateTime InsertedDate { get; set; }
        public virtual string UpdatedUser { get; set; }
        public virtual string UpdatedIp { get; set; }
        public virtual DateTime? UpdatedDate { get; set; }

        public EntityBaseAudit()
        {
            TrackId = Guid.NewGuid();
            State = StateType.Active;
        }
    }
}
