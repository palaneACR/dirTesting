using Shared.DataTransferObject.Localizer;

namespace Contracts
{
    public interface ILocalizerProcessor
    {
        Task<LocalizerDTO> GetCalculatedResultAsync(string filePath);
    }
}