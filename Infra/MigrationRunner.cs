using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoCheck.Infra
{
    public class MigrationRunner(DatabaseConnection databaseConnection)
    {
        private void createMigrationTable()
        {
            databaseConnection.PerformQuery(command =>
            {
                command.CommandText = "CREATE TABLE IF NOT EXISTS migrations(id INTEGER PRIMARY KEY NOT NULL, filename TEXT NOT NULL)";
                int result = command.ExecuteNonQuery();
                return result;
            });
        }
        private List<string> filterMigrationsNotApplied(string[] migrationFiles)
        {
            var filenames = migrationFiles.Select(migrationFile => Path.GetFileName(migrationFile));
            var filenamesString = string.Join(", ", filenames.Select(fn => $"\"{fn}\""));
            var filenameApplied = (List<string>) databaseConnection.PerformQuery(command =>
            {
                command.CommandText = $"SELECT filename FROM migrations WHERE filename IN ({filenamesString})";
                var r = command.ExecuteReader();
                List<string> names = new List<string>();
                while (r.Read())
                {
                    names.Add(r.GetString("filename"));
                }
                return names;
            });
            return filenameApplied;
        }
        private IEnumerable<string> getMigrationFilesNotApplied()
        {
            string[] migrationFiles = Directory.GetFiles("Migrations");
            List<string> migrationFilesApplied = filterMigrationsNotApplied(migrationFiles);
            return migrationFiles.Where(mf => !migrationFilesApplied.Contains(Path.GetFileName(mf)));
        }
        private void applyMigrations(IEnumerable<string> migrationFiles)
        {
            databaseConnection.PerformQuery(command =>
            {
                foreach (var migrationFile in migrationFiles)
                {
                    var data = File.ReadAllText(migrationFile);
                    command.CommandText = data;
                    command.ExecuteNonQuery();
                    Console.WriteLine($"Migration {migrationFile} applied");
                }
                return null;
            });
        }
        public void MigrateAll()
        {
            bool tablesAvailable = (bool) databaseConnection.PerformQuery(command =>
            {
                command.CommandText = "SELECT name FROM sqlite_schema WHERE type='table' ORDER BY name;";
                using (var r = command.ExecuteReader())
                {
                    if(!r.Read())
                    {
                        return false;
                    }
                    return true;
                }
            });
            if (!tablesAvailable) createMigrationTable();
            IEnumerable<string> migrationFiles = getMigrationFilesNotApplied();
            if (migrationFiles.Count() == 0) Console.WriteLine("No migrations to apply");
            applyMigrations(migrationFiles);
        }
    }
}
