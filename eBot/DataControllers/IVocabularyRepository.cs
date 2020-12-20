using System.Threading.Tasks;

namespace eBot.DataControllers
{
    public interface IVocabularyRepository
    {
        Task LoadEssentialVocabularySetAsync();
    }
}