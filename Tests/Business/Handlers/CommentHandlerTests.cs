
using Business.Handlers.Comments.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Comments.Queries.GetCommentQuery;
using Entities.Concrete;
using static Business.Handlers.Comments.Queries.GetCommentsQuery;
using static Business.Handlers.Comments.Commands.CreateCommentCommand;
using Business.Handlers.Comments.Commands;
using Business.Constants;
using static Business.Handlers.Comments.Commands.UpdateCommentCommand;
using static Business.Handlers.Comments.Commands.DeleteCommentCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class CommentHandlerTests
    {
        Mock<ICommentRepository> _commentRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _commentRepository = new Mock<ICommentRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Comment_GetQuery_Success()
        {
            //Arrange
            var query = new GetCommentQuery();

            _commentRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Comment, bool>>>())).ReturnsAsync(new Comment()
//propertyler buraya yazılacak
//{																		
//CommentId = 1,
//CommentName = "Test"
//}
);

            var handler = new GetCommentQueryHandler(_commentRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.CommentId.Should().Be(1);

        }

        [Test]
        public async Task Comment_GetQueries_Success()
        {
            //Arrange
            var query = new GetCommentsQuery();

            _commentRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Comment, bool>>>()))
                        .ReturnsAsync(new List<Comment> { new Comment() { /*TODO:propertyler buraya yazılacak CommentId = 1, CommentName = "test"*/ } });

            var handler = new GetCommentsQueryHandler(_commentRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Comment>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Comment_CreateCommand_Success()
        {
            Comment rt = null;
            //Arrange
            var command = new CreateCommentCommand();
            //propertyler buraya yazılacak
            //command.CommentName = "deneme";

            _commentRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Comment, bool>>>()))
                        .ReturnsAsync(rt);

            _commentRepository.Setup(x => x.Add(It.IsAny<Comment>())).Returns(new Comment());

            var handler = new CreateCommentCommandHandler(_commentRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _commentRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Comment_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateCommentCommand();
            //propertyler buraya yazılacak 
            //command.CommentName = "test";

            _commentRepository.Setup(x => x.Query())
                                           .Returns(new List<Comment> { new Comment() { /*TODO:propertyler buraya yazılacak CommentId = 1, CommentName = "test"*/ } }.AsQueryable());

            _commentRepository.Setup(x => x.Add(It.IsAny<Comment>())).Returns(new Comment());

            var handler = new CreateCommentCommandHandler(_commentRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Comment_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateCommentCommand();
            //command.CommentName = "test";

            _commentRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Comment, bool>>>()))
                        .ReturnsAsync(new Comment() { /*TODO:propertyler buraya yazılacak CommentId = 1, CommentName = "deneme"*/ });

            _commentRepository.Setup(x => x.Update(It.IsAny<Comment>())).Returns(new Comment());

            var handler = new UpdateCommentCommandHandler(_commentRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _commentRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Comment_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteCommentCommand();

            _commentRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Comment, bool>>>()))
                        .ReturnsAsync(new Comment() { /*TODO:propertyler buraya yazılacak CommentId = 1, CommentName = "deneme"*/});

            _commentRepository.Setup(x => x.Delete(It.IsAny<Comment>()));

            var handler = new DeleteCommentCommandHandler(_commentRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _commentRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

