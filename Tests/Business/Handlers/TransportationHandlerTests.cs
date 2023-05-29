
using Business.Handlers.Transportations.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Transportations.Queries.GetTransportationQuery;
using Entities.Concrete;
using static Business.Handlers.Transportations.Queries.GetTransportationsQuery;
using static Business.Handlers.Transportations.Commands.CreateTransportationCommand;
using Business.Handlers.Transportations.Commands;
using Business.Constants;
using static Business.Handlers.Transportations.Commands.UpdateTransportationCommand;
using static Business.Handlers.Transportations.Commands.DeleteTransportationCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class TransportationHandlerTests
    {
        Mock<ITransportationRepository> _transportationRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _transportationRepository = new Mock<ITransportationRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Transportation_GetQuery_Success()
        {
            //Arrange
            var query = new GetTransportationQuery();

            _transportationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Transportation, bool>>>())).ReturnsAsync(new Transportation()
//propertyler buraya yazılacak
//{																		
//TransportationId = 1,
//TransportationName = "Test"
//}
);

            var handler = new GetTransportationQueryHandler(_transportationRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.TransportationId.Should().Be(1);

        }

        [Test]
        public async Task Transportation_GetQueries_Success()
        {
            //Arrange
            var query = new GetTransportationsQuery();

            _transportationRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Transportation, bool>>>()))
                        .ReturnsAsync(new List<Transportation> { new Transportation() { /*TODO:propertyler buraya yazılacak TransportationId = 1, TransportationName = "test"*/ } });

            var handler = new GetTransportationsQueryHandler(_transportationRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Transportation>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Transportation_CreateCommand_Success()
        {
            Transportation rt = null;
            //Arrange
            var command = new CreateTransportationCommand();
            //propertyler buraya yazılacak
            //command.TransportationName = "deneme";

            _transportationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Transportation, bool>>>()))
                        .ReturnsAsync(rt);

            _transportationRepository.Setup(x => x.Add(It.IsAny<Transportation>())).Returns(new Transportation());

            var handler = new CreateTransportationCommandHandler(_transportationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _transportationRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Transportation_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateTransportationCommand();
            //propertyler buraya yazılacak 
            //command.TransportationName = "test";

            _transportationRepository.Setup(x => x.Query())
                                           .Returns(new List<Transportation> { new Transportation() { /*TODO:propertyler buraya yazılacak TransportationId = 1, TransportationName = "test"*/ } }.AsQueryable());

            _transportationRepository.Setup(x => x.Add(It.IsAny<Transportation>())).Returns(new Transportation());

            var handler = new CreateTransportationCommandHandler(_transportationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Transportation_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateTransportationCommand();
            //command.TransportationName = "test";

            _transportationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Transportation, bool>>>()))
                        .ReturnsAsync(new Transportation() { /*TODO:propertyler buraya yazılacak TransportationId = 1, TransportationName = "deneme"*/ });

            _transportationRepository.Setup(x => x.Update(It.IsAny<Transportation>())).Returns(new Transportation());

            var handler = new UpdateTransportationCommandHandler(_transportationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _transportationRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Transportation_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteTransportationCommand();

            _transportationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Transportation, bool>>>()))
                        .ReturnsAsync(new Transportation() { /*TODO:propertyler buraya yazılacak TransportationId = 1, TransportationName = "deneme"*/});

            _transportationRepository.Setup(x => x.Delete(It.IsAny<Transportation>()));

            var handler = new DeleteTransportationCommandHandler(_transportationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _transportationRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

