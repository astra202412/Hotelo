using Hotelo.Core.Interfaces.Services;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Hotelo.Infrastructure.Services;

public class BackupService : IBackupService
{
    private readonly IConfiguration        _config;
    private readonly ILogger<BackupService> _logger;
    private readonly string                _backupPath;
    private readonly string                _connectionString;

    public BackupService(IConfiguration config, ILogger<BackupService> logger)
    {
        _config           = config;
        _logger           = logger;
        _backupPath       = config["AppSettings:BackupPath"] ?? "C:\\Projects\\Hotelo\\Backups";
        _connectionString = config.GetConnectionString("HoteloConnection") ?? "";
    }

    public async Task<string> CreateBackupAsync()
    {
        try
        {
            if (!Directory.Exists(_backupPath))
                Directory.CreateDirectory(_backupPath);

            var fileName  = $"hoteloDB_backup_{DateTime.Now:yyyyMMdd_HHmmss}.bak";
            var fullPath  = Path.Combine(_backupPath, fileName);

            // Sans COMPRESSION — compatible SQL Server Express
            var backupSql = $"BACKUP DATABASE [hoteloDB] TO DISK = N'{fullPath}' WITH STATS = 10";

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(backupSql, conn);
            cmd.CommandTimeout = 300;
            await cmd.ExecuteNonQueryAsync();

            _logger.LogInformation("Backup cree : {FileName}", fileName);
            return fileName;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors du backup");
            throw;
        }
    }

    public async Task<List<string>> GetBackupListAsync()
    {
        return await Task.Run(() =>
        {
            if (!Directory.Exists(_backupPath))
                return new List<string>();

            return Directory.GetFiles(_backupPath, "*.bak")
                .OrderByDescending(f => File.GetCreationTime(f))
                .Select(f => Path.GetFileName(f))
                .ToList();
        });
    }

    public async Task<bool> RestoreBackupAsync(string backupFile)
    {
        try
        {
            var fullPath   = Path.Combine(_backupPath, backupFile);
            var restoreSql = $@"
                ALTER DATABASE [hoteloDB] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
                RESTORE DATABASE [hoteloDB] FROM DISK = N'{fullPath}' WITH REPLACE, STATS = 10;
                ALTER DATABASE [hoteloDB] SET MULTI_USER;";

            await using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var cmd = new SqlCommand(restoreSql, conn);
            cmd.CommandTimeout = 600;
            await cmd.ExecuteNonQueryAsync();

            _logger.LogInformation("Restauration reussie : {BackupFile}", backupFile);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur lors de la restauration");
            return false;
        }
    }
}
