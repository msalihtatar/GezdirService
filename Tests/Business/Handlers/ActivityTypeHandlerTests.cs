
using Business.Handlers.ActivityTypes.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.ActivityTypes.Queries.GetActivityTypeQuery;
using Entities.Concrete;
using static Business.Handlers.ActivityTypes.Queries.GetActivityTypesQuery;
using static Business.Handlers.ActivityTypes.Commands.CreateActivityTypeCommand;
using Business.Handlers.ActivityTypes.Commands;
using Business.Constants;
using static Business.Handlers.ActivityTypes.Commands.UpdateActivityTypeCommand;
using static Business.Handlers.ActivityTypes.Commands.DeleteActivityTypeCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class ActivityTypeHandlerTests
    {
        Mock<IActivityTypeRepository> _activityTypeRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _activityTypeRepository = new Mock<IActivityTypeRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task ActivityType_GetQuery_Success()
        {
            //Arrange
            var query = new GetActivityTypeQuery();

            _activityTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ActivityType, bool>>>())).ReturnsAsync(new ActivityType()
//propertyler buraya yazılacak
//{																		
//ActivityTypeId = 1,
//ActivityTypeName = "Test"
//}
);

            var handler = new GetActivityTypeQueryHandler(_activityTypeRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.ActivityTypeId.Should().Be(1);

        }

        [Test]
        public async Task ActivityType_GetQueries_Success()
        {
            //Arrange
            var query = new GetActivityTypesQuery();

            _activityTypeRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<ActivityType, bool>>>()))
                        .ReturnsAsync(new List<ActivityType> { new ActivityType() { /*TODO:propertyler buraya yazılacak ActivityTypeId = 1, ActivityTypeName = "test"*/ } });

            var handler = new GetActivityTypesQueryHandler(_activityTypeRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<ActivityType>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task ActivityType_CreateCommand_Success()
        {
            ActivityType rt = null;
            //Arrange
            var command = new CreateActivityTypeCommand();
            //propertyler buraya yazılacak
            //command.ActivityTypeName = "deneme";

            _activityTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ActivityType, bool>>>()))
                        .ReturnsAsync(rt);

            _activityTypeRepository.Setup(x => x.Add(It.IsAny<ActivityType>())).Returns(new ActivityType());

            var handler = new CreateActivityTypeCommandHandler(_activityTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _activityTypeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task ActivityType_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateActivityTypeCommand();
            //propertyler buraya yazılacak 
            //command.ActivityTypeName = "test";

            _activityTypeRepository.Setup(x => x.Query())
                                           .Returns(new List<ActivityType> { new ActivityType() { /*TODO:propertyler buraya yazılacak ActivityTypeId = 1, ActivityTypeName = "test"*/ } }.AsQueryable());

            _activityTypeRepository.Setup(x => x.Add(It.IsAny<ActivityType>())).Returns(new ActivityType());

            var handler = new CreateActivityTypeCommandHandler(_activityTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task ActivityType_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateActivityTypeCommand();
            //command.ActivityTypeName = "test";

            _activityTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ActivityType, bool>>>()))
                        .ReturnsAsync(new ActivityType() { /*TODO:propertyler buraya yazılacak ActivityTypeId = 1, ActivityTypeName = "deneme"*/ });

            _activityTypeRepository.Setup(x => x.Update(It.IsAny<ActivityType>())).Returns(new ActivityType());

            var handler = new UpdateActivityTypeCommandHandler(_activityTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _activityTypeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task ActivityType_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteActivityTypeCommand();

            _activityTypeRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<ActivityType, bool>>>()))
                        .ReturnsAsync(new ActivityType() { /*TODO:propertyler buraya yazılacak ActivityTypeId = 1, ActivityTypeName = "deneme"*/});

            _activityTypeRepository.Setup(x => x.Delete(It.IsAny<ActivityType>()));

            var handler = new DeleteActivityTypeCommandHandler(_activityTypeRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _activityTypeRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

