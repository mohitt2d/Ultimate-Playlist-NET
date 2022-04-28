namespace UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces
{
    public interface IApplyCriteriaVisitor<out T>
    {
        T Apply(ICriteriaVisitor criteriaVisitor);
    }
}
