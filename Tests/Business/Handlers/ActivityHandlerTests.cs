
using Business.Handlers.Activities.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Activities.Queries.GetActivityQuery;
using Entities.Concrete;
using static Business.Handlers.Activities.Queries.GetActivitiesQuery;
using static Business.Handlers.Activities.Commands.CreateActivityCommand;
using Business.Handlers.Activities.Commands;
using Business.Constants;
using static Business.Handlers.Activities.Commands.UpdateActivityCommand;
using static Business.Handlers.Activities.Commands.DeleteActivityCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class ActivityHandlerTests
    {
        Mock<IActivityRepository> _activityRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _activityRepository = new Mock<IActivityRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Activity_GetQuery_Success()
        {
            //Arrange
            var query = new GetActivityQuery();

            _activityRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Activity, bool>>>())).ReturnsAsync(new Activity()
//propertyler buraya yazılacak
//{																		
//ActivityId = 1,
//ActivityName = "Test"
//}
);

            var handler = new GetActivityQueryHandler(_activityRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.ActivityId.Should().Be(1);

        }

        [Test]
        public async Task Activity_GetQueries_Success()
        {
            //Arrange
            var query = new GetActivitiesQuery();

            _activityRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Activity, bool>>>()))
                        .ReturnsAsync(new List<Activity> { new Activity() { /*TODO:propertyler buraya yazılacak ActivityId = 1, ActivityName = "test"*/ } });

            var handler = new GetActivitiesQueryHandler(_activityRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Activity>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Activity_CreateCommand_Success()
        {
            Activity rt = null;
            //Arrange
            var command = new CreateActivityCommand();
            //propertyler buraya yazılacak
            //command.ActivityName = "deneme";

            _activityRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Activity, bool>>>()))
                        .ReturnsAsync(rt);

            _activityRepository.Setup(x => x.Add(It.IsAny<Activity>())).Returns(new Activity());

            var handler = new CreateActivityCommandHandler(_activityRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _activityRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Activity_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateActivityCommand();
            //propertyler buraya yazılacak 
            //command.ActivityName = "test";

            _activityRepository.Setup(x => x.Query())
                                           .Returns(new List<Activity> { new Activity() { /*TODO:propertyler buraya yazılacak ActivityId = 1, ActivityName = "test"*/ } }.AsQueryable());

            _activityRepository.Setup(x => x.Add(It.IsAny<Activity>())).Returns(new Activity());

            var handler = new CreateActivityCommandHandler(_activityRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Activity_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateActivityCommand();
            //command.ActivityName = "test";

            _activityRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Activity, bool>>>()))
                        .ReturnsAsync(new Activity() { /*TODO:propertyler buraya yazılacak ActivityId = 1, ActivityName = "deneme"*/ });

            _activityRepository.Setup(x => x.Update(It.IsAny<Activity>())).Returns(new Activity());

            var handler = new UpdateActivityCommandHandler(_activityRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _activityRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Activity_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteActivityCommand();

            _activityRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Activity, bool>>>()))
                        .ReturnsAsync(new Activity() { /*TODO:propertyler buraya yazılacak ActivityId = 1, ActivityName = "deneme"*/});

            _activityRepository.Setup(x => x.Delete(It.IsAny<Activity>()));

            var handler = new DeleteActivityCommandHandler(_activityRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _activityRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

