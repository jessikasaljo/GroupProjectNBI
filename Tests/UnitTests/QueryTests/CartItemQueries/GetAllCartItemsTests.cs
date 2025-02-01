

using Application.Queries.CartItemQueries.GetAllCartItems;
using Domain.Models;
using Domain.RepositoryInterface;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.UnitTests.QueryTests.CartItemQueries
{
    public class GetAllCartItemsTests
    {
        private readonly Mock<IGenericRepository<Cart>> _mockCartRepo;
        private readonly Mock<ILogger<GetAllCartItemsQueryHandler>> _mockLogger;
        private readonly GetAllCartItemsQueryHandler _handler;

        public GetAllCartItemsTests()
        {
            _mockCartRepo = new Mock<IGenericRepository<Cart>>();
            _mockLogger = new Mock<ILogger<GetAllCartItemsQueryHandler>>();
            _handler = new GetAllCartItemsQueryHandler(_mockCartRepo.Object, _mockLogger.Object);
        }


        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenCartFound()
        {
            var query = new GetAllCartItemsQuery(1);
            var cart = new Cart
            {
                Id = 1,
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 2, Product = new Product { Name = "Product1", Price = 10 } },
                    new CartItem { ProductId = 2, Quantity = 1, Product = new Product { Name = "Product2", Price = 15 } }
                }
            };

            _mockCartRepo.Setup(repo => repo.QueryAsync(It.IsAny<Func<IQueryable<Cart>, IQueryable<Cart>>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<Cart> { cart });

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.True(result.Success);
            Assert.Equal(2, result.Data!.Count());
            Assert.Equal(20, result.Data!.First().ProductPrice * result.Data.First().Quantity);
        }
    }
}
