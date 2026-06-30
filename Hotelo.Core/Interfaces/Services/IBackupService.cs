namespace Hotelo.Core.Interfaces.Services;

public interface IBackupService
{
    Task<string> CreateBackupAsync();
    Task<List<string>> GetBackupListAsync();
    Task<bool> RestoreBackupAsync(string backupFile);
}
