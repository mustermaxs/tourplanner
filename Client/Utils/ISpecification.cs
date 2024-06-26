using System.Linq.Expressions;

namespace Client.Utils.Specifications
{
    public interface ISpecification<T>
    {
        bool IsSatisfiedBy(T candidate);
        Task<bool> IsSatisfiedByAsync(T candidate);
        ISpecification<T> And(ISpecification<T> other);
        ISpecification<T> AndNot(ISpecification<T> other);
        ISpecification<T> Or(ISpecification<T> other);
        ISpecification<T> OrNot(ISpecification<T> other);
        ISpecification<T> Not();
    }

    public abstract class LinqSpecification<T> : CompositeSpecification<T>
    {
        public abstract Expression<Func<T, bool>> AsExpression();
        public override bool IsSatisfiedBy(T candidate) => AsExpression().Compile()(candidate);

        public override Task<bool> IsSatisfiedByAsync(T candidate)
        {
            return Task.FromResult(IsSatisfiedBy(candidate));
        }
    }

    public abstract class CompositeSpecification<T> : ISpecification<T>
    {
        public abstract bool IsSatisfiedBy(T candidate);

        public virtual Task<bool> IsSatisfiedByAsync(T candidate)
        {
            return Task.FromResult(IsSatisfiedBy(candidate));
        }

        public ISpecification<T> And(ISpecification<T> other) => new AndSpecification<T>(this, other);
        public ISpecification<T> AndNot(ISpecification<T> other) => new AndNotSpecification<T>(this, other);
        public ISpecification<T> Or(ISpecification<T> other) => new OrSpecification<T>(this, other);
        public ISpecification<T> OrNot(ISpecification<T> other) => new OrNotSpecification<T>(this, other);
        public ISpecification<T> Not() => new NotSpecification<T>(this);
    }

    public class AndSpecification<T> : CompositeSpecification<T>
    {
        ISpecification<T> left;
        ISpecification<T> right;

        public AndSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public override bool IsSatisfiedBy(T candidate) =>
            left.IsSatisfiedBy(candidate) && right.IsSatisfiedBy(candidate);

        public override async Task<bool> IsSatisfiedByAsync(T candidate) => await left.IsSatisfiedByAsync(candidate) &&
                                                                            await right.IsSatisfiedByAsync(candidate);
    }

    public class AndNotSpecification<T> : CompositeSpecification<T>
    {
        ISpecification<T> left;
        ISpecification<T> right;

        public AndNotSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public override bool IsSatisfiedBy(T candidate) =>
            left.IsSatisfiedBy(candidate) && !right.IsSatisfiedBy(candidate);

        public override async Task<bool> IsSatisfiedByAsync(T candidate) => await left.IsSatisfiedByAsync(candidate) &&
                                                                            !await right.IsSatisfiedByAsync(candidate);
    }

    public class OrSpecification<T> : CompositeSpecification<T>
    {
        ISpecification<T> left;
        ISpecification<T> right;

        public OrSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public override bool IsSatisfiedBy(T candidate) =>
            left.IsSatisfiedBy(candidate) || right.IsSatisfiedBy(candidate);

        public override async Task<bool> IsSatisfiedByAsync(T candidate) => await left.IsSatisfiedByAsync(candidate) ||
                                                                            await right.IsSatisfiedByAsync(candidate);
    }

    public class OrNotSpecification<T> : CompositeSpecification<T>
    {
        ISpecification<T> left;
        ISpecification<T> right;

        public OrNotSpecification(ISpecification<T> left, ISpecification<T> right)
        {
            this.left = left;
            this.right = right;
        }

        public override bool IsSatisfiedBy(T candidate) =>
            left.IsSatisfiedBy(candidate) || !right.IsSatisfiedBy(candidate);

        public override async Task<bool> IsSatisfiedByAsync(T candidate) => await left.IsSatisfiedByAsync(candidate) ||
                                                                            !await right.IsSatisfiedByAsync(candidate);
    }

    public class NotSpecification<T> : CompositeSpecification<T>
    {
        ISpecification<T> other;
        public NotSpecification(ISpecification<T> other) => this.other = other;
        public override bool IsSatisfiedBy(T candidate) => !other.IsSatisfiedBy(candidate);
        public override async Task<bool> IsSatisfiedByAsync(T candidate) => !await other.IsSatisfiedByAsync(candidate);
    }

    public class StringNotEmptySpecification : LinqSpecification<string>
    {
        public override Expression<Func<string, bool>> AsExpression()
        {
            return candidate => !string.IsNullOrEmpty(candidate);
        }
    }

    public class IsTrueSpecification : LinqSpecification<bool>
    {
        public override Expression<Func<bool, bool>> AsExpression()
        {
            return candidate => candidate;
        }
    }

    public class ValueInRangeSpecification<T>(T minValue, T maxValue) : LinqSpecification<T>
        where T : IComparable<T>
    {
        public override Expression<Func<T, bool>> AsExpression()
        {
            return candidate => candidate.CompareTo(minValue) >= 0 && candidate.CompareTo(maxValue) <= 0;
        }
    }
}