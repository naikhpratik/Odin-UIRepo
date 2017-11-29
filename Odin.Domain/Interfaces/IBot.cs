using Odin.Domain.Bots.Dtos;

namespace Odin.Domain.Interfaces
{
    interface IBot
    {
        HousingPropertyDto Bot(string orderId);
        HousingPropertyImagesDto BotImages(string hpId);
    }
}
