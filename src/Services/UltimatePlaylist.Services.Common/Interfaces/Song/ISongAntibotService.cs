namespace UltimatePlaylist.Services.Common.Interfaces.Song
{
    public interface ISongAntibotService
    {
        Task<bool> ShouldActivateAsync(Guid userExternalId, CancellationToken cancellationToken = default);

        Task AddNoActionAsync(Guid userExternalId, CancellationToken cancellationToken = default);

        Task ResetCounterAsync(Guid userExternalId, CancellationToken cancellationToken = default);
    }
}
