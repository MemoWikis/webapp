using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Seedworks.Lib.Persistence
{
    /// <remarks>
    /// Based on:
    /// https://github.com/Slesa/Playground/blob/master/src/lib/DataAccess/DataAccess/DomainEntity.cs
    /// </remarks>>
    [Serializable]
    public class Entity : IEquatable<Entity>, IPersistable
    {
        protected Entity()
        {
        }

        protected Entity(int id)
        {
            Id = id;
        }

        public virtual int Id { get; set; }

        public virtual bool Equals(Entity other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(default(int)) ? base.Equals(other) : other.Id.Equals(Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity);
        }

        public override int GetHashCode()
        {
            return Id.Equals(default(int)) ? base.GetHashCode() : Id.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !Equals(left, right);
        }
    }
}
