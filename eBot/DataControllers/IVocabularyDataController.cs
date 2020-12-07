using System.Threading.Tasks;

namespace eBot.DataControllers
{
    public interface IVocabularyDataController
    {
        Task LoadEssentialVocabularySetAsync();
    }
}