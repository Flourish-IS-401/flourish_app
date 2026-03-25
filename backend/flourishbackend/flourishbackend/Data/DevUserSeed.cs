using Flourish.Models;
using Microsoft.EntityFrameworkCore;

namespace flourishbackend.Data;

/// <summary>
/// Inserts a predictable local-dev user if missing, so FKs (e.g. MoodEntry.UserId → UserProfile) succeed.
/// </summary>
public static class DevUserSeed
{
    /// <summary>Stable id you can paste into the frontend for quick testing.</summary>
    public static readonly Guid DevUserId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    /// <summary>Stable baby row so BabyActivity / BabyMood FKs succeed in local dev.</summary>
    public static readonly Guid DevBabyId = Guid.Parse("22222222-2222-2222-2222-222222222222");

    public static void EnsureDevUser(FlourishDbContext db)
    {
        if (db.UserProfiles.AsNoTracking().Any(u => u.UserId == DevUserId))
            return;

        db.UserProfiles.Add(new UserProfile
        {
            UserId = DevUserId,
            Username = "dev_seeduser",
            UserFirstName = "Dev",
            UserLastName = "Seed",
            Password = "",
            PhoneNumber = "",
            NotificationsMoodEnabled = false,
            NotificationsMoodTimes = [],
            NotificationsFeedingEnabled = false,
            NotificationsFeedingTimes = [],
            NotificationsNapEnabled = false,
            NotificationsNapTimes = [],
            HomeFeatures =
            [
                "affirmation", "mood", "mood_chips", "mindfulness", "tasks",
                "baby", "support", "breathing", "journal", "meditations", "articles",
            ],
        });

        db.SaveChanges();
    }

    public static void EnsureDevBabyProfile(FlourishDbContext db)
    {
        if (db.BabyProfiles.AsNoTracking().Any(b => b.BabyId == DevBabyId))
            return;

        db.BabyProfiles.Add(new BabyProfile
        {
            BabyId = DevBabyId,
            BabyName = "Baby",
            BabyDateOfBirth = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
            UserId = DevUserId,
        });

        db.SaveChanges();
    }
}
