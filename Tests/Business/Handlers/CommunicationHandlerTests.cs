
using Business.Handlers.Communications.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Communications.Queries.GetCommunicationQuery;
using Entities.Concrete;
using static Business.Handlers.Communications.Queries.GetCommunicationsQuery;
using static Business.Handlers.Communications.Commands.CreateCommunicationCommand;
using Business.Handlers.Communications.Commands;
using Business.Constants;
using static Business.Handlers.Communications.Commands.UpdateCommunicationCommand;
using static Business.Handlers.Communications.Commands.DeleteCommunicationCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class CommunicationHandlerTests
    {
        Mock<ICommunicationRepository> _communicationRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _communicationRepository = new Mock<ICommunicationRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Communication_GetQuery_Success()
        {
            //Arrange
            var query = new GetCommunicationQuery();

            _communicationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Communication, bool>>>())).ReturnsAsync(new Communication()
//propertyler buraya yazılacak
//{																		
//CommunicationId = 1,
//CommunicationName = "Test"
//}
);

            var handler = new GetCommunicationQueryHandler(_communicationRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.CommunicationId.Should().Be(1);

        }

        [Test]
        public async Task Communication_GetQueries_Success()
        {
            //Arrange
            var query = new GetCommunicationsQuery();

            _communicationRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Communication, bool>>>()))
                        .ReturnsAsync(new List<Communication> { new Communication() { /*TODO:propertyler buraya yazılacak CommunicationId = 1, CommunicationName = "test"*/ } });

            var handler = new GetCommunicationsQueryHandler(_communicationRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Communication>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Communication_CreateCommand_Success()
        {
            Communication rt = null;
            //Arrange
            var command = new CreateCommunicationCommand();
            //propertyler buraya yazılacak
            //command.CommunicationName = "deneme";

            _communicationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Communication, bool>>>()))
                        .ReturnsAsync(rt);

            _communicationRepository.Setup(x => x.Add(It.IsAny<Communication>())).Returns(new Communication());

            var handler = new CreateCommunicationCommandHandler(_communicationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _communicationRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Communication_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateCommunicationCommand();
            //propertyler buraya yazılacak 
            //command.CommunicationName = "test";

            _communicationRepository.Setup(x => x.Query())
                                           .Returns(new List<Communication> { new Communication() { /*TODO:propertyler buraya yazılacak CommunicationId = 1, CommunicationName = "test"*/ } }.AsQueryable());

            _communicationRepository.Setup(x => x.Add(It.IsAny<Communication>())).Returns(new Communication());

            var handler = new CreateCommunicationCommandHandler(_communicationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Communication_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateCommunicationCommand();
            //command.CommunicationName = "test";

            _communicationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Communication, bool>>>()))
                        .ReturnsAsync(new Communication() { /*TODO:propertyler buraya yazılacak CommunicationId = 1, CommunicationName = "deneme"*/ });

            _communicationRepository.Setup(x => x.Update(It.IsAny<Communication>())).Returns(new Communication());

            var handler = new UpdateCommunicationCommandHandler(_communicationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _communicationRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Communication_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteCommunicationCommand();

            _communicationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Communication, bool>>>()))
                        .ReturnsAsync(new Communication() { /*TODO:propertyler buraya yazılacak CommunicationId = 1, CommunicationName = "deneme"*/});

            _communicationRepository.Setup(x => x.Delete(It.IsAny<Communication>()));

            var handler = new DeleteCommunicationCommandHandler(_communicationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _communicationRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

