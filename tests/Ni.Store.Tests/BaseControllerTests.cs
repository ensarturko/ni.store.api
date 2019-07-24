using Ni.Store.Api.Models.Responses;
using NUnit.Framework;
using FluentAssertions;
using Ni.Store.Api.Controllers;
using System.Collections.Generic;

namespace Ni.Store.Tests
{
    [TestFixture]
    public class BaseControllerTests
    {
        private BaseController _baseController;

        [SetUp]
        public void Initialize()
        {
            _baseController = new BaseController();
        }

        [Test]
        public void PreparePagination_WhenGivenOffsetAsAStartIndex_ShouldReturnResponseWithFirstPagePagination()
        {
            //Arrange
            int offset = 0;
            int limit = 10;
            int total = 100;
            string resource = "tests";
            PagedApiResponse<List<string>> pagedApiResponse = new PagedApiResponse<List<string>>();

            //Act
            _baseController.PreparePagination(offset, limit, total, resource, pagedApiResponse);

            //Assert
            pagedApiResponse.Index.Should().Be(1);
            pagedApiResponse.Next.Should().NotBeNullOrEmpty();
            pagedApiResponse.Prev.Should().BeNullOrEmpty();
        }

        [Test]
        public void PreprearePagination_WhenGivenOffsetAsA11_ShouldReturnResponseWithSecondPagePagination()
        {
            //Arrange
            int offset = 11;
            int limit = 10;
            int total = 100;
            string resource = "tests";
            PagedApiResponse<List<string>> pagedApiResponse = new PagedApiResponse<List<string>>();

            //Act
            _baseController.PreparePagination(offset, limit, total, resource, pagedApiResponse);

            //Assert
            pagedApiResponse.Index.Should().Be(2);
            pagedApiResponse.Next.Should().NotBeNullOrEmpty();
            pagedApiResponse.Prev.Should().NotBeNullOrEmpty();
        }
    }
}