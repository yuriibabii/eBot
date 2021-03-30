using System.Threading.Tasks;

namespace eBot.Repositories
{
    public interface IVocabularyRepository
    {
        Task LoadEssentialVocabularySetAsync();
    }
}