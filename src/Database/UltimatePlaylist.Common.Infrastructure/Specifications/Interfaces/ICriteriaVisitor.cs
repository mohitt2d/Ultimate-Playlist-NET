#region Usings

using System.Linq.Expressions;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Specifications.Interfaces
{
    public interface ICriteriaVisitor
    {
        Expression VisitCriteria(Expression criteria);
    }
}
