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

        public RequestHandler() {}

        public abstract Task<TResponse> Handle(TRequest request);
    }

    public interface IResponse
    {
    }

    public interface IPublisher
    {
    }
}