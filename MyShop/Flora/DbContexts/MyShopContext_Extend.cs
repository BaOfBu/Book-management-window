using System.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore;

namespace Flora;

public partial class MyShopContext : DbContext
{
    public void BackupDatabase(string backupPath)
    {
        // Get the database name
        string databaseName = Database.GetDbConnection().Database;

        // Execute raw SQL command to perform backup
        Database.ExecuteSqlRaw($"BACKUP DATABASE [{databaseName}] TO DISK = '{backupPath}' WITH INIT");
    }
}

