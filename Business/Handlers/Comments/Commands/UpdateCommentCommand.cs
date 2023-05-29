
using Business.Constants;
using Business.BusinessAspects;
using Core.Aspects.Autofac.Caching;
using Core.Aspects.Autofac.Logging;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Core.Aspects.Autofac.Validation;
using Business.Handlers.Comments.ValidationRules;
using DataAccess.Abstract;

namespace Business.Handlers.Comments.Commands
{


    public class UpdateCommentCommand : IRequest<IResult>
    {
        public int CommentId { get; set; }
        public int LocationId { get; set; }
        public string Comment1 { get; set; }

        public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, IResult>
        {
            private readonly ICommentRepository _commentRepository;
            private readonly IMediator _mediator;

            public UpdateCommentCommandHandler(ICommentRepository commentRepository, IMediator mediator)
            {
                _commentRepository = commentRepository;
                _mediator = mediator;
            }

            [ValidationAspect(typeof(UpdateCommentValidator), Priority = 1)]
            [CacheRemoveAspect("Get")]
            [LogAspect(typeof(FileLogger))]
            [SecuredOperation(Priority = 1)]
            public async Task<IResult> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
            {
                var isThereCommentRecord = await _commentRepository.GetAsync(u => u.CommentId == request.CommentId);


                isThereCommentRecord.LocationId = request.LocationId;
                isThereCommentRecord.CommentContent = request.Comment1;


                _commentRepository.Update(isThereCommentRecord);
                await _commentRepository.SaveChangesAsync();
                return new SuccessResult(Messages.Updated);
            }
        }
    }
}

