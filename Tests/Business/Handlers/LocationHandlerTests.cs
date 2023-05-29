
using Business.Handlers.Locations.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Locations.Queries.GetLocationQuery;
using Entities.Concrete;
using static Business.Handlers.Locations.Queries.GetLocationsQuery;
using static Business.Handlers.Locations.Commands.CreateLocationCommand;
using Business.Handlers.Locations.Commands;
using Business.Constants;
using static Business.Handlers.Locations.Commands.UpdateLocationCommand;
using static Business.Handlers.Locations.Commands.DeleteLocationCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class LocationHandlerTests
    {
        Mock<ILocationRepository> _locationRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _locationRepository = new Mock<ILocationRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Location_GetQuery_Success()
        {
            //Arrange
            var query = new GetLocationQuery();

            _locationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Location, bool>>>())).ReturnsAsync(new Location()
//propertyler buraya yazılacak
//{																		
//LocationId = 1,
//LocationName = "Test"
//}
);

            var handler = new GetLocationQueryHandler(_locationRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.LocationId.Should().Be(1);

        }

        [Test]
        public async Task Location_GetQueries_Success()
        {
            //Arrange
            var query = new GetLocationsQuery();

            _locationRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                        .ReturnsAsync(new List<Location> { new Location() { /*TODO:propertyler buraya yazılacak LocationId = 1, LocationName = "test"*/ } });

            var handler = new GetLocationsQueryHandler(_locationRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Location>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Location_CreateCommand_Success()
        {
            Location rt = null;
            //Arrange
            var command = new CreateLocationCommand();
            //propertyler buraya yazılacak
            //command.LocationName = "deneme";

            _locationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                        .ReturnsAsync(rt);

            _locationRepository.Setup(x => x.Add(It.IsAny<Location>())).Returns(new Location());

            var handler = new CreateLocationCommandHandler(_locationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _locationRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Location_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateLocationCommand();
            //propertyler buraya yazılacak 
            //command.LocationName = "test";

            _locationRepository.Setup(x => x.Query())
                                           .Returns(new List<Location> { new Location() { /*TODO:propertyler buraya yazılacak LocationId = 1, LocationName = "test"*/ } }.AsQueryable());

            _locationRepository.Setup(x => x.Add(It.IsAny<Location>())).Returns(new Location());

            var handler = new CreateLocationCommandHandler(_locationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Location_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateLocationCommand();
            //command.LocationName = "test";

            _locationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                        .ReturnsAsync(new Location() { /*TODO:propertyler buraya yazılacak LocationId = 1, LocationName = "deneme"*/ });

            _locationRepository.Setup(x => x.Update(It.IsAny<Location>())).Returns(new Location());

            var handler = new UpdateLocationCommandHandler(_locationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _locationRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Location_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteLocationCommand();

            _locationRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Location, bool>>>()))
                        .ReturnsAsync(new Location() { /*TODO:propertyler buraya yazılacak LocationId = 1, LocationName = "deneme"*/});

            _locationRepository.Setup(x => x.Delete(It.IsAny<Location>()));

            var handler = new DeleteLocationCommandHandler(_locationRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _locationRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

