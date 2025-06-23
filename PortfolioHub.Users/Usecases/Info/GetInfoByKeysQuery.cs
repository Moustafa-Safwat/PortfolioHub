using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using PortfolioHub.Users.Endpoints.Info;

namespace PortfolioHub.Users.Usecases.Info;

internal sealed record GetInfoByKeysQuery(
    IEnumerable<string> Keys
    ) : IRequest<Result<IEnumerable<InfoGetDto>>>;
