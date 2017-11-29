namespace Odin.PropBotWebJob.Interfaces
{
    public interface IBot
    {
        HousingPropertyDto Bot(string orderId);
        HousingPropertyImagesDto BotImages(string hpId);
    }
}
