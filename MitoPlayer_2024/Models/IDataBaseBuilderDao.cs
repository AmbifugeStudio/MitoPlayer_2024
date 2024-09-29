using MitoPlayer_2024.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Models
{
    public interface IDataBaseBuilderDao
    {
        bool IsConnectionStringValid(String preConnectionString);
        bool IsDatabaseExists(String preConnectionString);
        ResultOrError CreateDatabase(String preConnectionString);
        ResultOrError CreateTableStructure();
        ResultOrError DeleteDatabase();

    }
}
