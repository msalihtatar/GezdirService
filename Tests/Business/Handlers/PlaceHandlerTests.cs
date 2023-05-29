
using Business.Handlers.Places.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Places.Queries.GetPlaceQuery;
using Entities.Concrete;
using static Business.Handlers.Places.Queries.GetPlacesQuery;
using static Business.Handlers.Places.Commands.CreatePlaceCommand;
using Business.Handlers.Places.Commands;
using Business.Constants;
using static Business.Handlers.Places.Commands.UpdatePlaceCommand;
using static Business.Handlers.Places.Commands.DeletePlaceCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class PlaceHandlerTests
    {
        Mock<IPlaceRepository> _placeRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _placeRepository = new Mock<IPlaceRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Place_GetQuery_Success()
        {
            //Arrange
            var query = new GetPlaceQuery();

            _placeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Place, bool>>>())).ReturnsAsync(new Place()
//propertyler buraya yazılacak
//{																		
//PlaceId = 1,
//PlaceName = "Test"
//}
);

            var handler = new GetPlaceQueryHandler(_placeRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.PlaceId.Should().Be(1);

        }

        [Test]
        public async Task Place_GetQueries_Success()
        {
            //Arrange
            var query = new GetPlacesQuery();

            _placeRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Place, bool>>>()))
                        .ReturnsAsync(new List<Place> { new Place() { /*TODO:propertyler buraya yazılacak PlaceId = 1, PlaceName = "test"*/ } });

            var handler = new GetPlacesQueryHandler(_placeRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Place>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Place_CreateCommand_Success()
        {
            Place rt = null;
            //Arrange
            var command = new CreatePlaceCommand();
            //propertyler buraya yazılacak
            //command.PlaceName = "deneme";

            _placeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Place, bool>>>()))
                        .ReturnsAsync(rt);

            _placeRepository.Setup(x => x.Add(It.IsAny<Place>())).Returns(new Place());

            var handler = new CreatePlaceCommandHandler(_placeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _placeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Place_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreatePlaceCommand();
            //propertyler buraya yazılacak 
            //command.PlaceName = "test";

            _placeRepository.Setup(x => x.Query())
                                           .Returns(new List<Place> { new Place() { /*TODO:propertyler buraya yazılacak PlaceId = 1, PlaceName = "test"*/ } }.AsQueryable());

            _placeRepository.Setup(x => x.Add(It.IsAny<Place>())).Returns(new Place());

            var handler = new CreatePlaceCommandHandler(_placeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Place_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdatePlaceCommand();
            //command.PlaceName = "test";

            _placeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Place, bool>>>()))
                        .ReturnsAsync(new Place() { /*TODO:propertyler buraya yazılacak PlaceId = 1, PlaceName = "deneme"*/ });

            _placeRepository.Setup(x => x.Update(It.IsAny<Place>())).Returns(new Place());

            var handler = new UpdatePlaceCommandHandler(_placeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _placeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Place_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeletePlaceCommand();

            _placeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Place, bool>>>()))
                        .ReturnsAsync(new Place() { /*TODO:propertyler buraya yazılacak PlaceId = 1, PlaceName = "deneme"*/});

            _placeRepository.Setup(x => x.Delete(It.IsAny<Place>()));

            var handler = new DeletePlaceCommandHandler(_placeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _placeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

