using System.Data;

namespace MixMate.Core.Interfaces
{
    public interface IDatabaseContext
    {
        IDbConnection CreateConnection();
    }
}