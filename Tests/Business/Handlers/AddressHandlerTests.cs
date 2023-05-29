
using Business.Handlers.Addresses.Queries;
using DataAccess.Abstract;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using static Business.Handlers.Addresses.Queries.GetAddressQuery;
using Entities.Concrete;
using static Business.Handlers.Addresses.Queries.GetAddressesQuery;
using static Business.Handlers.Addresses.Commands.CreateAddressCommand;
using Business.Handlers.Addresses.Commands;
using Business.Constants;
using static Business.Handlers.Addresses.Commands.UpdateAddressCommand;
using static Business.Handlers.Addresses.Commands.DeleteAddressCommand;
using MediatR;
using System.Linq;
using FluentAssertions;


namespace Tests.Business.HandlersTest
{
    [TestFixture]
    public class AddressHandlerTests
    {
        Mock<IAddressRepository> _addressRepository;
        Mock<IMediator> _mediator;
        [SetUp]
        public void Setup()
        {
            _addressRepository = new Mock<IAddressRepository>();
            _mediator = new Mock<IMediator>();
        }

        [Test]
        public async Task Address_GetQuery_Success()
        {
            //Arrange
            var query = new GetAddressQuery();

            _addressRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Address, bool>>>())).ReturnsAsync(new Address()
//propertyler buraya yazılacak
//{																		
//AddressId = 1,
//AddressName = "Test"
//}
);

            var handler = new GetAddressQueryHandler(_addressRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            //x.Data.AddressId.Should().Be(1);

        }

        [Test]
        public async Task Address_GetQueries_Success()
        {
            //Arrange
            var query = new GetAddressesQuery();

            _addressRepository.Setup(x => x.GetListAsync(It.IsAny<Expression<Func<Address, bool>>>()))
                        .ReturnsAsync(new List<Address> { new Address() { /*TODO:propertyler buraya yazılacak AddressId = 1, AddressName = "test"*/ } });

            var handler = new GetAddressesQueryHandler(_addressRepository.Object, _mediator.Object);

            //Act
            var x = await handler.Handle(query, new System.Threading.CancellationToken());

            //Asset
            x.Success.Should().BeTrue();
            ((List<Address>)x.Data).Count.Should().BeGreaterThan(1);

        }

        [Test]
        public async Task Address_CreateCommand_Success()
        {
            Address rt = null;
            //Arrange
            var command = new CreateAddressCommand();
            //propertyler buraya yazılacak
            //command.AddressName = "deneme";

            _addressRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Address, bool>>>()))
                        .ReturnsAsync(rt);

            _addressRepository.Setup(x => x.Add(It.IsAny<Address>())).Returns(new Address());

            var handler = new CreateAddressCommandHandler(_addressRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _addressRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Added);
        }

        [Test]
        public async Task Address_CreateCommand_NameAlreadyExist()
        {
            //Arrange
            var command = new CreateAddressCommand();
            //propertyler buraya yazılacak 
            //command.AddressName = "test";

            _addressRepository.Setup(x => x.Query())
                                           .Returns(new List<Address> { new Address() { /*TODO:propertyler buraya yazılacak AddressId = 1, AddressName = "test"*/ } }.AsQueryable());

            _addressRepository.Setup(x => x.Add(It.IsAny<Address>())).Returns(new Address());

            var handler = new CreateAddressCommandHandler(_addressRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            x.Success.Should().BeFalse();
            x.Message.Should().Be(Messages.NameAlreadyExist);
        }

        [Test]
        public async Task Address_UpdateCommand_Success()
        {
            //Arrange
            var command = new UpdateAddressCommand();
            //command.AddressName = "test";

            _addressRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Address, bool>>>()))
                        .ReturnsAsync(new Address() { /*TODO:propertyler buraya yazılacak AddressId = 1, AddressName = "deneme"*/ });

            _addressRepository.Setup(x => x.Update(It.IsAny<Address>())).Returns(new Address());

            var handler = new UpdateAddressCommandHandler(_addressRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _addressRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Updated);
        }

        [Test]
        public async Task Address_DeleteCommand_Success()
        {
            //Arrange
            var command = new DeleteAddressCommand();

            _addressRepository.Setup(x => x.GetAsync(It.IsAny<Expression<Func<Address, bool>>>()))
                        .ReturnsAsync(new Address() { /*TODO:propertyler buraya yazılacak AddressId = 1, AddressName = "deneme"*/});

            _addressRepository.Setup(x => x.Delete(It.IsAny<Address>()));

            var handler = new DeleteAddressCommandHandler(_addressRepository.Object, _mediator.Object);
            var x = await handler.Handle(command, new System.Threading.CancellationToken());

            _addressRepository.Verify(x => x.SaveChangesAsync());
            x.Success.Should().BeTrue();
            x.Message.Should().Be(Messages.Deleted);
        }
    }
}

