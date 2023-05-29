
using Business.Handlers.TransportationTypes.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.TransportationTypes.Queries.GetTransportationTypeQuery;
using Entities.Concrete;
using static Business.Handlers.TransportationTypes.Queries.GetTransportationTypesQuery;
using static Business.Handlers.TransportationTypes.Commands.CreateTransportationTypeCommand;
using Business.Handlers.TransportationTypes.Commands;
using Business.Constants;
using static Business.Handlers.TransportationTypes.Commands.UpdateTransportationTypeCommand;
using static Business.Handlers.TransportationTypes.Commands.DeleteTransportationTypeCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class TransportationTypeHandlerTests
    {
        Mock<ITransportationTypeRepository> _transportationTypeRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _transportationTypeRepository = new Mock<ITransportationTypeRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task TransportationType_GetQuery_Success()
        {
            //Arrange
            var query = new GetTransportationTypeQuery();

            _transportationTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TransportationType, bool>>>())).ReturnsAsync(new TransportationType()
//propertyler buraya yazılacak
//{																		
//TransportationTypeId = 1,
//TransportationTypeName = "Test"
//}
);

            var handler = new GetTransportationTypeQueryHandler(_transportationTypeRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.TransportationTypeId.Should().Be(1);

        }

        [Test]
        public async Task TransportationType_GetQueries_Success()
        {
            //Arrange
            var query = new GetTransportationTypesQuery();

            _transportationTypeRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<TransportationType, bool>>>()))
                        .ReturnsAsync(new List<TransportationType> { new TransportationType() { /*TODO:propertyler buraya yazılacak TransportationTypeId = 1, TransportationTypeName = "test"*/ } });

            var handler = new GetTransportationTypesQueryHandler(_transportationTypeRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<TransportationType>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task TransportationType_CreateCommand_Success()
        {
            TransportationType rt = null;
            //Arrange
            var command = new CreateTransportationTypeCommand();
            //propertyler buraya yazılacak
            //command.TransportationTypeName = "deneme";

            _transportationTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TransportationType, bool>>>()))
                        .ReturnsAsync(rt);

            _transportationTypeRepository.Setup(x => x.Add(It.IsAny<TransportationType>())).Returns(new TransportationType());

            var handler = new CreateTransportationTypeCommandHandler(_transportationTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _transportationTypeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task TransportationType_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateTransportationTypeCommand();
            //propertyler buraya yazılacak 
            //command.TransportationTypeName = "test";

            _transportationTypeRepository.Setup(x => x.Query())
                                           .Returns(new List<TransportationType> { new TransportationType() { /*TODO:propertyler buraya yazılacak TransportationTypeId = 1, TransportationTypeName = "test"*/ } }.AsQueryable());

            _transportationTypeRepository.Setup(x => x.Add(It.IsAny<TransportationType>())).Returns(new TransportationType());

            var handler = new CreateTransportationTypeCommandHandler(_transportationTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task TransportationType_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateTransportationTypeCommand();
            //command.TransportationTypeName = "test";

            _transportationTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TransportationType, bool>>>()))
                        .ReturnsAsync(new TransportationType() { /*TODO:propertyler buraya yazılacak TransportationTypeId = 1, TransportationTypeName = "deneme"*/ });

            _transportationTypeRepository.Setup(x => x.Update(It.IsAny<TransportationType>())).Returns(new TransportationType());

            var handler = new UpdateTransportationTypeCommandHandler(_transportationTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _transportationTypeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task TransportationType_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteTransportationTypeCommand();

            _transportationTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<TransportationType, bool>>>()))
                        .ReturnsAsync(new TransportationType() { /*TODO:propertyler buraya yazılacak TransportationTypeId = 1, TransportationTypeName = "deneme"*/});

            _transportationTypeRepository.Setup(x => x.Delete(It.IsAny<TransportationType>()));

            var handler = new DeleteTransportationTypeCommandHandler(_transportationTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _transportationTypeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

