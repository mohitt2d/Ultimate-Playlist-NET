#region Usings

using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Filters;
using UltimatePlaylist.Database.Infrastructure.Context;

#endregion

namespace UltimatePlaylist.Common.Mvc.Attributes
{
    public class EFTransactionAttribute : ActionFilterAttribute
    {
        #region Private Members

        private readonly EFContext context;

        #endregion

        #region Constructor(s)

        public EFTransactionAttribute(EFContext context)
        {
            this.context = context;
        }

        #endregion

        #region Protected members

        protected virtual TransactionOptions DefaultTransactionOptions
        {
            get
            {
                return new TransactionOptions
                {
                    IsolationLevel = IsolationLevel.ReadCommitted,
                };
            }
        }

        #endregion

        #region ActionFilterAttribute Override(s)

        public override async Task OnActionExecutionAsync(ActionExecutingContext executingContext, ActionExecutionDelegate next)
        {
            using (var transaction = await context.Database.BeginTransactionAsync())
            {
                var executedContext = await next.Invoke();
                if (executedContext.Exception == null)
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
        }

        #endregion
    }
}
