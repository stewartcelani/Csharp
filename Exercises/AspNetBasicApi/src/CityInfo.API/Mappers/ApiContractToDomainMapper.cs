using System;
using CityInfo.API.Contracts.v1.Requests;
using CityInfo.API.Contracts.v1.Requests.Queries;
using CityInfo.API.Domain;
using CityInfo.API.Domain.Filters;

namespace CityInfo.API.Mappers;

public static class ApiContractToDomainMapper
{
    public static PointOfInterest ToPointOfInterest(this CreatePointOfInterestRequest request)
    {
        return new PointOfInterest
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };
    }

    public static PointOfInterest ToPointOfInterest(this UpdatePointOfInterestRequest request)
    {
        return new PointOfInterest
        {
            Id = request.Id,
            Name = request.CreatePointOfInterestRequest.Name,
            Description = request.CreatePointOfInterestRequest.Description
        };
    }

    public static City ToCity(this CreateCityRequest request)
    {
        return new City
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description
        };
    }

    public static City ToCity(this UpdateCityRequest request)
    {
        return new City
        {
            Id = request.Id,
            Name = request.CreateCityRequest.Name,
            Description = request.CreateCityRequest.Description
        };
    }

    public static GetCitiesFilter ToGetCitiesFilter(this GetCitiesQuery getCitiesQuery)
    {
        return new GetCitiesFilter
        {
            Name = getCitiesQuery.Name
        };
    }

    public static PaginationFilter ToPaginationFilter(this PaginationQuery paginationQuery)
    {
        return new PaginationFilter
        {
            PageNumber = paginationQuery.PageNumber,
            PageSize = paginationQuery.PageSize
        };
    }
}