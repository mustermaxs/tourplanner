using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourplanner.DTOs;

namespace Tourplanner.Infrastructure
{
    public interface ICommandHandler
    {
    }

    public abstract class RequestHandler<TRequest, TResponse> : ICommandHandler
        where TRequest : IRequest
    {
        protected TourContext dbContext;

        public RequestHandler(TourContext context)
        {
            dbContext = context;
        }

        public abstract Task<TResponse> Handle(TRequest request);
    }

    public interface IResponse
    {
    }

    public interface IPublisher
    {
    }
}