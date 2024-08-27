using CineTix.Models.Interfaces;

namespace CineTix.Models.Repositories;

public class PricingRepository : IPricingRepository
{
    public decimal GetPrice(int movieId, DateTime showTime)
    {
        throw new NotImplementedException();
    }
}