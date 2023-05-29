
using Business.Handlers.Scores.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Scores.Queries.GetScoreQuery;
using Entities.Concrete;
using static Business.Handlers.Scores.Queries.GetScoresQuery;
using static Business.Handlers.Scores.Commands.CreateScoreCommand;
using Business.Handlers.Scores.Commands;
using Business.Constants;
using static Business.Handlers.Scores.Commands.UpdateScoreCommand;
using static Business.Handlers.Scores.Commands.DeleteScoreCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class ScoreHandlerTests
    {
        Mock<IScoreRepository> _scoreRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _scoreRepository = new Mock<IScoreRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Score_GetQuery_Success()
        {
            //Arrange
            var query = new GetScoreQuery();

            _scoreRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Score, bool>>>())).ReturnsAsync(new Score()
//propertyler buraya yazılacak
//{																		
//ScoreId = 1,
//ScoreName = "Test"
//}
);

            var handler = new GetScoreQueryHandler(_scoreRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.ScoreId.Should().Be(1);

        }

        [Test]
        public async Task Score_GetQueries_Success()
        {
            //Arrange
            var query = new GetScoresQuery();

            _scoreRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Score, bool>>>()))
                        .ReturnsAsync(new List<Score> { new Score() { /*TODO:propertyler buraya yazılacak ScoreId = 1, ScoreName = "test"*/ } });

            var handler = new GetScoresQueryHandler(_scoreRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Score>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Score_CreateCommand_Success()
        {
            Score rt = null;
            //Arrange
            var command = new CreateScoreCommand();
            //propertyler buraya yazılacak
            //command.ScoreName = "deneme";

            _scoreRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Score, bool>>>()))
                        .ReturnsAsync(rt);

            _scoreRepository.Setup(x => x.Add(It.IsAny<Score>())).Returns(new Score());

            var handler = new CreateScoreCommandHandler(_scoreRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _scoreRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Score_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateScoreCommand();
            //propertyler buraya yazılacak 
            //command.ScoreName = "test";

            _scoreRepository.Setup(x => x.Query())
                                           .Returns(new List<Score> { new Score() { /*TODO:propertyler buraya yazılacak ScoreId = 1, ScoreName = "test"*/ } }.AsQueryable());

            _scoreRepository.Setup(x => x.Add(It.IsAny<Score>())).Returns(new Score());

            var handler = new CreateScoreCommandHandler(_scoreRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Score_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateScoreCommand();
            //command.ScoreName = "test";

            _scoreRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Score, bool>>>()))
                        .ReturnsAsync(new Score() { /*TODO:propertyler buraya yazılacak ScoreId = 1, ScoreName = "deneme"*/ });

            _scoreRepository.Setup(x => x.Update(It.IsAny<Score>())).Returns(new Score());

            var handler = new UpdateScoreCommandHandler(_scoreRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _scoreRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Score_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteScoreCommand();

            _scoreRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Score, bool>>>()))
                        .ReturnsAsync(new Score() { /*TODO:propertyler buraya yazılacak ScoreId = 1, ScoreName = "deneme"*/});

            _scoreRepository.Setup(x => x.Delete(It.IsAny<Score>()));

            var handler = new DeleteScoreCommandHandler(_scoreRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _scoreRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

