using TransactionApi.Entities;

namespace TransactionApi.Interfaces
{
    public interface ICsvService
    {
        void ReadFile(IFormFile file);
    }
}