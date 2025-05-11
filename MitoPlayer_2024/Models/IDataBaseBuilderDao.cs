using MitoPlayer_2024.Helpers.ErrorHandling;
using System;

namespace MitoPlayer_2024.Models
{
    public interface IDataBaseBuilderDao
    {
        bool IsConnectionStringValid(String preConnectionString);
        bool IsDatabaseExists(String preConnectionString);
        ResultOrError CreateDatabase(String preConnectionString);
        ResultOrError CreateTableStructure();
        ResultOrError DeleteDatabase(String databaseFilePath);

    }
}
