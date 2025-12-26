using System;
using System.Collections.Generic;

namespace Bermuda.Core.Database.Entity;

public abstract class EntityBase<PKey> : IEquatable<EntityBase<PKey>>
{
    public virtual PKey Id { get; protected set; }

    protected EntityBase()
    {
        // Generate Guid PK if empty (prevents PK collisions from Guid.Empty)
        if (typeof(PKey) == typeof(Guid) && EqualityComparer<PKey>.Default.Equals(Id, default!))
        {
            Id = (PKey)(object)Guid.NewGuid();
        }
    }
    protected EntityBase(PKey id) => Id = id;

    public bool IsTransient() =>
        EqualityComparer<PKey>.Default.Equals(Id, default!);

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;

        return Equals(obj as EntityBase<PKey>);
    }

    public bool Equals(EntityBase<PKey>? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (other.GetType() != GetType()) return false;

        // Do not consider two transient entities equal
        if (IsTransient() || other.IsTransient()) return false;

        return EqualityComparer<PKey>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode() =>
        IsTransient()
            ? base.GetHashCode()
            : EqualityComparer<PKey>.Default.GetHashCode(Id);

    public static bool operator ==(EntityBase<PKey>? left, EntityBase<PKey>? right)
    {
        if (ReferenceEquals(left, right)) return true;
        if (left is null || right is null) return false;
        return left.Equals(right);
    }

    public static bool operator !=(EntityBase<PKey>? left, EntityBase<PKey>? right) =>
        !(left == right);

    public override string ToString() => $"{GetType().Name} [Id={Id}]";
}
