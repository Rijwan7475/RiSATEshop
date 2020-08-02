using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;
using RiSAT.Eshop.Application.Common.Interfaces;
using RiSAT.Eshop.Common;
using RiSAT.Eshop.Domain.Entities;
using RiSAT.Eshop.Persistence;
using Shouldly;
using Xunit;

namespace Persistence.IntegrationTests
{
    public class RiSATeshopDbContextTests : IDisposable
    {
        private readonly string _userId;
        private readonly DateTime _dateTime;
        private readonly Mock<IDateTime> _dateTimeMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly RiSATeshopDbContext _sut;

        public RiSATeshopDbContextTests()
        {
            _dateTime = new DateTime(3001, 1, 1);
            _dateTimeMock = new Mock<IDateTime>();
            _dateTimeMock.Setup(m => m.Now).Returns(_dateTime);

            _userId = "00000000-0000-0000-0000-000000000000";
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _currentUserServiceMock.Setup(m => m.UserId).Returns(_userId);

            var options = new DbContextOptionsBuilder<RiSATeshopDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _sut = new RiSATeshopDbContext(options, _currentUserServiceMock.Object, _dateTimeMock.Object);

            _sut.Products.Add(new Product
            {
                ProductId = 1, 
                ProductName = "Coffee"
            });

            _sut.SaveChanges();
        }

        [Fact]
        public async Task SaveChangesAsync_GivenNewProduct_ShouldSetCreatedProperties()
        {
            var product = new Product
            {
                ProductId = 2,
                ProductName = "Cake"
            };

            _sut.Products.Add(product);

            await _sut.SaveChangesAsync();

            product.Created.ShouldBe(_dateTime);
            product.CreatedBy.ShouldBe(_userId);
        }

        [Fact]
        public async Task SaveChangesAsync_GivenExistingProduct_ShouldSetLastModifiedProperties()
        {
            var product = await _sut.Products.FindAsync(1);

            product.UnitPrice = 4m;

            await _sut.SaveChangesAsync();

            product.LastModified.ShouldNotBeNull();
            product.LastModified.ShouldBe(_dateTime);
            product.LastModifiedBy.ShouldBe(_userId);
        }

        public void Dispose()
        {
            _sut?.Dispose();
        }
    }
}
