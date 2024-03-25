using TransactionApi.Entities;

namespace TransactionApi.Interfaces
{
    public interface ICsvImportService
    {
        void ReadFile(IFormFile file);
    }
}