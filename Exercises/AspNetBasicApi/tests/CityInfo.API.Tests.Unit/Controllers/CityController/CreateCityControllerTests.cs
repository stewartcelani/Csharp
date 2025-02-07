using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using CityInfo.API.Contracts.v1.Requests;
using CityInfo.API.Contracts.v1.Responses;
using CityInfo.API.Domain;
using CityInfo.API.Exceptions;
using CityInfo.API.Mappers;
using CityInfo.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Xunit;

namespace CityInfo.API.Tests.Unit.Controllers.CityController;

[ExcludeFromCodeCoverage]
public class CreateCityControllerTests
{
    private readonly ICityService _cityService = Substitute.For<ICityService>();
    private readonly API.Controllers.v1.CityController _sut;


    public CreateCityControllerTests()
    {
        _sut = new API.Controllers.v1.CityController(_cityService, new UriService("http://localhost/"));
    }

    [Fact]
    public async Task CreateCity_ShouldCreateCity_WhenCreateCityRequestIsValid()
    {
        // Arrange
        var createCityRequest = new CreateCityRequest
        {
            Name = "City Name",
            Description = "City description."
        };
        var city = createCityRequest.ToCity();
        _cityService.CreateAsync(Arg.Do<City>(x => city = x)).Returns(true);

        // Act
        var result = (CreatedAtActionResult)await _sut.CreateCity(createCityRequest);

        // Assert
        var expectedCityResponse = city.ToCityResponse();
        result.StatusCode.Should().Be(201);
        result.Value.As<CityResponse>().Should().BeEquivalentTo(expectedCityResponse);
        result.RouteValues!["cityId"].Should().BeEquivalentTo(city.Id);
    }

    /// <summary>
    ///     Note the ApiException is translated into a BadRequest 400 with error message by middleware
    /// </summary>
    [Fact]
    public async Task CreateCity_ShouldThrowApiException_WhenCreateCityRequestIsInvalid()
    {
        // Arrange
        _cityService.CreateAsync(Arg.Any<City>()).Returns(false);

        // Act
        var action = async () => await _sut.CreateCity(new CreateCityRequest());

        // Assert
        await action.Should().ThrowAsync<ApiException>().WithMessage("Error creating city*");
    }
}