
using Business.Handlers.UserPreferences.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.UserPreferences.Queries.GetUserPreferenceQuery;
using Entities.Concrete;
using static Business.Handlers.UserPreferences.Queries.GetUserPreferencesQuery;
using static Business.Handlers.UserPreferences.Commands.CreateUserPreferenceCommand;
using Business.Handlers.UserPreferences.Commands;
using Business.Constants;
using static Business.Handlers.UserPreferences.Commands.UpdateUserPreferenceCommand;
using static Business.Handlers.UserPreferences.Commands.DeleteUserPreferenceCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class UserPreferenceHandlerTests
    {
        Mock<IUserPreferenceRepository> _userPreferenceRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _userPreferenceRepository = new Mock<IUserPreferenceRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task UserPreference_GetQuery_Success()
        {
            //Arrange
            var query = new GetUserPreferenceQuery();

            _userPreferenceRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPreference, bool>>>())).ReturnsAsync(new UserPreference()
            //propertyler buraya yazılacak
            //{																		
            //UserPreferenceId = 1,
            //UserPreferenceName = "Test"
            //}
            );

            var handler = new GetUserPreferenceQueryHandler(_userPreferenceRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.UserPreferenceId.Should().Be(1);

        }

        [Test]
        public async Task UserPreference_GetQueries_Success()
        {
            //Arrange
            var query = new GetUserPreferencesQuery();

            _userPreferenceRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<UserPreference, bool>>>()))
                        .ReturnsAsync(new List<UserPreference> { new UserPreference() { /*TODO:propertyler buraya yazılacak UserPreferenceId = 1, UserPreferenceName = "test"*/ } });

            var handler = new GetUserPreferencesQueryHandler(_userPreferenceRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<UserPreference>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task UserPreference_CreateCommand_Success()
        {
            UserPreference rt = null;
            //Arrange
            var command = new CreateUserPreferenceCommand();
            //propertyler buraya yazılacak
            //command.UserPreferenceName = "deneme";

            _userPreferenceRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPreference, bool>>>()))
                        .ReturnsAsync(rt);

            _userPreferenceRepository.Setup(x => x.Add(It.IsAny<UserPreference>())).Returns(new UserPreference());

            var handler = new CreateUserPreferenceCommandHandler(_userPreferenceRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _userPreferenceRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task UserPreference_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateUserPreferenceCommand();
            //propertyler buraya yazılacak 
            //command.UserPreferenceName = "test";

            _userPreferenceRepository.Setup(x => x.Query())
                                           .Returns(new List<UserPreference> { new UserPreference() { /*TODO:propertyler buraya yazılacak UserPreferenceId = 1, UserPreferenceName = "test"*/ } }.AsQueryable());

            _userPreferenceRepository.Setup(x => x.Add(It.IsAny<UserPreference>())).Returns(new UserPreference());

            var handler = new CreateUserPreferenceCommandHandler(_userPreferenceRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task UserPreference_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateUserPreferenceCommand();
            //command.UserPreferenceName = "test";

            _userPreferenceRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPreference, bool>>>()))
                        .ReturnsAsync(new UserPreference() { /*TODO:propertyler buraya yazılacak UserPreferenceId = 1, UserPreferenceName = "deneme"*/ });

            _userPreferenceRepository.Setup(x => x.Update(It.IsAny<UserPreference>())).Returns(new UserPreference());

            var handler = new UpdateUserPreferenceCommandHandler(_userPreferenceRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _userPreferenceRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task UserPreference_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteUserPreferenceCommand();

            _userPreferenceRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<UserPreference, bool>>>()))
                        .ReturnsAsync(new UserPreference() { /*TODO:propertyler buraya yazılacak UserPreferenceId = 1, UserPreferenceName = "deneme"*/});

            _userPreferenceRepository.Setup(x => x.Delete(It.IsAny<UserPreference>()));

            var handler = new DeleteUserPreferenceCommandHandler(_userPreferenceRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _userPreferenceRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

