using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MitoPlayer_2024.Dao
{
    public abstract class BaseDao
    {
        protected string connectionString;

        public abstract int GetNextId(String tableName);

       
    }

}
