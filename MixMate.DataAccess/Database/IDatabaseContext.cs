using System.Data;

namespace MixMate.DataAccess.Database
{
    public interface IDatabaseContext
    {
        IDbConnection CreateConnection();
    }
}