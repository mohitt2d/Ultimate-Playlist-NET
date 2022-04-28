#region Usings

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using UltimatePlaylist.Database.Infrastructure.Views;

#endregion

namespace UltimatePlaylist.Database.Infrastructure.Entities.Song.Configuration
{
    internal class GeneralSongsCountProcedureViewConfiguration : IEntityTypeConfiguration<GeneralSongsCountProcedureView>
    {
        #region BuildEntity

        public virtual void Configure(EntityTypeBuilder<GeneralSongsCountProcedureView> builder)
        {
            builder.HasNoKey();
        }

        #endregion
    }
}
