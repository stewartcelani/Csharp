﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Bogus;
using CityInfo.API.Domain;
using CityInfo.API.Domain.Entities;
using CityInfo.API.Mappers;
using CityInfo.API.Repositories;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;
using ValidationException = FluentValidation.ValidationException;

namespace CityInfo.API.Tests.Unit.CityService;

[ExcludeFromCodeCoverage]
public class CreateCityServiceTests
{
    private readonly Services.CityService _sut;
    private readonly ICityRepository _cityRepository = Substitute.For<ICityRepository>();
    private readonly Faker<City> _cityGenerator;

    public CreateCityServiceTests()
    {
        _sut = new Services.CityService(_cityRepository);
        _cityGenerator = SharedTestContext.CityGenerator;
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateCity_WhenCityIsValid()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var cityEntity = city.ToCityEntity();
        _cityRepository.ExistsAsync(city.Id).Returns(false);
        _cityRepository.CreateAsync(Arg.Do<CityEntity>(x => cityEntity = x)).Returns(true);

        // Act
        var result = await _sut.CreateAsync(city);

        // Assert
        cityEntity.Should().BeEquivalentTo(city);
        result.Should().BeTrue();
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowValidationException_WhenCityAlreadyExists()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        _cityRepository.ExistsAsync(city.Id).Returns(true);

        // Act
        var action = async () => await _sut.CreateAsync(city);

        // Assert
        await action.Should().ThrowAsync<ValidationException>().WithMessage($"A city with id {city.Id} already exists");
    }
    
    [Fact]
    public async Task CreateAsync_ShouldReturnFalse_WhenRepositoryCouldNotChangeDatabase()
    {
        // Arrange
        var city = _cityGenerator.Generate();
        var cityEntity = city.ToCityEntity();
        _cityRepository.ExistsAsync(city.Id).Returns(false);
        _cityRepository.CreateAsync(Arg.Do<CityEntity>(x => cityEntity = x)).Returns(false);

        // Act
        var result = await _sut.CreateAsync(city);

        // Assert
        cityEntity.Should().BeEquivalentTo(city);
        result.Should().BeFalse();
    }
    
}