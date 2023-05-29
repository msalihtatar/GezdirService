
using Business.Handlers.PlaceTypes.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.PlaceTypes.Queries.GetPlaceTypeQuery;
using Entities.Concrete;
using static Business.Handlers.PlaceTypes.Queries.GetPlaceTypesQuery;
using static Business.Handlers.PlaceTypes.Commands.CreatePlaceTypeCommand;
using Business.Handlers.PlaceTypes.Commands;
using Business.Constants;
using static Business.Handlers.PlaceTypes.Commands.UpdatePlaceTypeCommand;
using static Business.Handlers.PlaceTypes.Commands.DeletePlaceTypeCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class PlaceTypeHandlerTests
    {
        Mock<IPlaceTypeRepository> _placeTypeRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _placeTypeRepository = new Mock<IPlaceTypeRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task PlaceType_GetQuery_Success()
        {
            //Arrange
            var query = new GetPlaceTypeQuery();

            _placeTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<PlaceType, bool>>>())).ReturnsAsync(new PlaceType()
//propertyler buraya yazılacak
//{																		
//PlaceTypeId = 1,
//PlaceTypeName = "Test"
//}
);

            var handler = new GetPlaceTypeQueryHandler(_placeTypeRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.PlaceTypeId.Should().Be(1);

        }

        [Test]
        public async Task PlaceType_GetQueries_Success()
        {
            //Arrange
            var query = new GetPlaceTypesQuery();

            _placeTypeRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<PlaceType, bool>>>()))
                        .ReturnsAsync(new List<PlaceType> { new PlaceType() { /*TODO:propertyler buraya yazılacak PlaceTypeId = 1, PlaceTypeName = "test"*/ } });

            var handler = new GetPlaceTypesQueryHandler(_placeTypeRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<PlaceType>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task PlaceType_CreateCommand_Success()
        {
            PlaceType rt = null;
            //Arrange
            var command = new CreatePlaceTypeCommand();
            //propertyler buraya yazılacak
            //command.PlaceTypeName = "deneme";

            _placeTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<PlaceType, bool>>>()))
                        .ReturnsAsync(rt);

            _placeTypeRepository.Setup(x => x.Add(It.IsAny<PlaceType>())).Returns(new PlaceType());

            var handler = new CreatePlaceTypeCommandHandler(_placeTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _placeTypeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task PlaceType_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreatePlaceTypeCommand();
            //propertyler buraya yazılacak 
            //command.PlaceTypeName = "test";

            _placeTypeRepository.Setup(x => x.Query())
                                           .Returns(new List<PlaceType> { new PlaceType() { /*TODO:propertyler buraya yazılacak PlaceTypeId = 1, PlaceTypeName = "test"*/ } }.AsQueryable());

            _placeTypeRepository.Setup(x => x.Add(It.IsAny<PlaceType>())).Returns(new PlaceType());

            var handler = new CreatePlaceTypeCommandHandler(_placeTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task PlaceType_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdatePlaceTypeCommand();
            //command.PlaceTypeName = "test";

            _placeTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<PlaceType, bool>>>()))
                        .ReturnsAsync(new PlaceType() { /*TODO:propertyler buraya yazılacak PlaceTypeId = 1, PlaceTypeName = "deneme"*/ });

            _placeTypeRepository.Setup(x => x.Update(It.IsAny<PlaceType>())).Returns(new PlaceType());

            var handler = new UpdatePlaceTypeCommandHandler(_placeTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _placeTypeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task PlaceType_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeletePlaceTypeCommand();

            _placeTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<PlaceType, bool>>>()))
                        .ReturnsAsync(new PlaceType() { /*TODO:propertyler buraya yazılacak PlaceTypeId = 1, PlaceTypeName = "deneme"*/});

            _placeTypeRepository.Setup(x => x.Delete(It.IsAny<PlaceType>()));

            var handler = new DeletePlaceTypeCommandHandler(_placeTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _placeTypeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

