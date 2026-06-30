using Hotelo.Core.Interfaces.Services;

namespace Hotelo.Web.Jobs;

public class BackupJob
{
    private readonly IBackupService _backupService;
    private readonly ILogger<BackupJob> _logger;

    public BackupJob(IBackupService backupService, ILogger<BackupJob> logger)
    {
        _backupService = backupService;
        _logger = logger;
    }

    public async Task ExecuteAsync()
    {
        try
        {
            var fileName = await _backupService.CreateBackupAsync();
            _logger.LogInformation("Backup automatique reussi : {FileName}", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erreur backup automatique");
        }
    }
}
