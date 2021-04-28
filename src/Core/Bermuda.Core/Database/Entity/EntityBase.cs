namespace Bermuda.Core.Database.Entity
{
    public class EntityBase<PKey>
    {
        public virtual PKey Id { get; set; }
    }
}
