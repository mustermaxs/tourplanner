using Client.Models;
using Client.Utils.Specifications;

namespace Client.Utils;

public class TourLogSpecification: CompositeSpecification<TourLog>
{
    public override bool IsSatisfiedBy(TourLog log)
    {
        return (
            log.Difficulty is >= 0 and <= 10 &&
            log.Rating is >= 0 and <= 10  &&
            log.Distance >= 0
        );
    }
}