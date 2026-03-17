using Microsoft.EntityFrameworkCore;
using Flourish.Models;
namespace flourishbackend.Data
{
    public class FlourishDbContext: DbContext
    {
        public FlourishDbContext(DbContextOptions<FlourishDbContext> options) : base(options) { 
        
        }

        public DbSet<AffirmationReaction> AffirmationReactions { get; set; }
        public DbSet<BabyActivity> BabyActivities { get; set; }
        public DbSet<BabyMood> BabyMoods { get; set; }
        public DbSet<CustomAffirmation> CustomAffirmations { get; set; }
        public DbSet<MoodEntry> MoodEntries { get; set; }
        public DbSet<SelectedSupportRequest> SelectedSupportRequests { get; set; }
        public DbSet<SupportRequest> SupportRequests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
