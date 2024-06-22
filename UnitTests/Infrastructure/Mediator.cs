using NUnit.Framework;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Tourplanner.Infrastructure;
using Tourplanner.Exceptions;

namespace Tourplanner.UnitTests.Infrastructure
{
    public class TestRequest : IRequest { }

    public class TestResponse { }
    public class TestRequestHandler : RequestHandler<TestRequest, TestResponse>
    {
        public override Task<TestResponse> Handle(TestRequest request)
        {
            return Task.FromResult(new TestResponse());
        }
    }


    [TestFixture]
    public class MediatorTests
    {
        private ServiceProvider _serviceProvider;
        private Mediator _mediator;
        private Mock<IRequest> _mockRequest;

        [SetUp]
        public void SetUp()
        {
            var services = new ServiceCollection();
            services.AddTransient<ICommandHandler, TestRequestHandler>();
            _serviceProvider = services.BuildServiceProvider();

            _mediator = new Mediator(_serviceProvider);

            _mockRequest = new Mock<IRequest>();
        }

        [TearDown]
        public void TearDown()
        {
            _serviceProvider.Dispose();
            GetCommandHandlerMapping().Clear();
        }

        private Dictionary<string, Type> GetCommandHandlerMapping()
        {
            var fieldInfo = typeof(IMediator).GetField("_CommandCommandHandlerMapping", BindingFlags.NonPublic | BindingFlags.Static);
            return (Dictionary<string, Type>)fieldInfo.GetValue(null);
        }

        [Test]
        public void Register_ShouldAddCommandAndHandlerToMapping()
        {
            IMediator.DiscoverPublishers(Assembly.GetExecutingAssembly());

            Assert.That(GetCommandHandlerMapping(), Has.Count.EqualTo(1));
        }


        [Test]
        public void Send_ShouldInvokeHandler()
        {
            IMediator.DiscoverPublishers(Assembly.GetExecutingAssembly());

            var request = new TestRequest();
            var response = _mediator.Send(request).Result;

            Assert.That(response, Is.InstanceOf<TestResponse>());
        }

        [Test]
        public void Send_ShouldThrowExceptionWhenSendingUnknownCommand()
        {
            var ex = Assert.ThrowsAsync<InfrastructureException>(async () => await _mediator.Send(_mockRequest.Object));
            Assert.That(ex.Message, Is.EqualTo("[Mediator] Command IRequestProxy unknown"));
        }   
    

        [Test]
        public void DiscoverPublishers_ShouldRegisterHandlers()
        {
            IMediator.DiscoverPublishers(Assembly.GetExecutingAssembly());

            Assert.That(GetCommandHandlerMapping(), Has.Count.EqualTo(1));
        }

    }
}
