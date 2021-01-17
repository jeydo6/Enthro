using Enthro.Application.Models;
using MediatR;

namespace Enthro.Application.Queries
{
    public class GetAnthropometryValueQuery : IRequest<AnthropometryValueModel>
    {
        public GetAnthropometryValueQuery(AnthropometryModel model)
        {
            Model = model;
        }

        public AnthropometryModel Model { get; }
    }
}
