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

    /// <summary>Email for the seeded developer account (sign-in uses email + password).</summary>
    public const string DevUserEmail = "developer@flourish.local";

    /// <summary>Second stable dev account for multi-user testing.</summary>
    public static readonly Guid SecondTestUserId = Guid.Parse("33333333-3333-3333-3333-333333333333");

    public const string SecondTestUserEmail = "test2@flourish.local";

    public static void EnsureDevUser(FlourishDbContext db)
    {
        if (db.UserProfiles.AsNoTracking().Any(u => u.UserId == DevUserId))
            return;

        db.UserProfiles.Add(new UserProfile
        {
            UserId = DevUserId,
            Username = "dev_seeduser",
            Email = DevUserEmail,
            UserFirstName = "Dev",
            UserLastName = "Seed",
            Password = "dev",
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

    public static void EnsureSecondTestUser(FlourishDbContext db)
    {
        if (db.UserProfiles.AsNoTracking().Any(u => u.UserId == SecondTestUserId))
            return;

        db.UserProfiles.Add(new UserProfile
        {
            UserId = SecondTestUserId,
            Username = "testuser2",
            Email = SecondTestUserEmail,
            UserFirstName = "Test",
            UserLastName = "UserTwo",
            Password = "dev",
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

    /// <summary>Fills <see cref="UserProfile.Email"/> on the dev row if it was created before that column existed.</summary>
    public static void EnsureDevUserEmail(FlourishDbContext db)
    {
        var dev = db.UserProfiles.FirstOrDefault(u => u.UserId == DevUserId);
        if (dev == null)
            return;

        var changed = false;
        if (string.IsNullOrWhiteSpace(dev.Email))
        {
            dev.Email = DevUserEmail;
            changed = true;
        }

        if (changed)
            db.SaveChanges();
    }

    /// <summary>Fills <see cref="UserProfile.Email"/> on the second test row if missing.</summary>
    public static void EnsureSecondTestUserEmail(FlourishDbContext db)
    {
        var row = db.UserProfiles.FirstOrDefault(u => u.UserId == SecondTestUserId);
        if (row == null)
            return;

        if (!string.IsNullOrWhiteSpace(row.Email))
            return;

        row.Email = SecondTestUserEmail;
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

    public static void EnsureDevAffirmations(FlourishDbContext db)
    {
        // Seed once (by exact text match) so local dev stays stable across restarts.
        var texts = GetDevAffirmationTexts();
        var existing = db.Affirmations.AsNoTracking()
            .Where(a => texts.Contains(a.Text))
            .Select(a => a.Text)
            .ToHashSet(StringComparer.Ordinal);

        var toAdd = texts
            .Where(t => !existing.Contains(t))
            .Select(t => new Affirmation { Text = t })
            .ToList();

        if (toAdd.Count == 0) return;
        db.Affirmations.AddRange(toAdd);
        db.SaveChanges();
    }

    public static void EnsureDevAffirmationReactions(FlourishDbContext db)
    {
        // Requires affirmations to exist.
        var affirmations = db.Affirmations.AsNoTracking()
            .OrderBy(a => a.Text)
            .Take(10)
            .ToList();
        if (affirmations.Count == 0) return;

        // Seed only if the dev user has no reactions yet (keep it simple + idempotent).
        if (db.AffirmationReactions.AsNoTracking().Any(r => r.UserId == DevUserId))
            return;

        var picks = affirmations.Take(5).ToList();
        var reactions = new[]
        {
            "up","up","up","down","up"
        };

        var toAdd = picks.Select((a, i) => new AffirmationReaction
        {
            AffirmationId = a.AffirmationId,
            UserId = DevUserId,
            Reaction = reactions[i]
        }).ToList();

        db.AffirmationReactions.AddRange(toAdd);
        db.SaveChanges();
    }

    public static void EnsureDevJournalEntries(FlourishDbContext db)
    {
        if (db.JournalEntries.AsNoTracking().Any(j => j.UserId == DevUserId))
            return;

        var now = DateTime.UtcNow;
        db.JournalEntries.AddRange(
            new JournalEntry
            {
                UserId = DevUserId,
                CreatedDate = now.AddDays(-2),
                Prompt = "What’s one small win from today?",
                Content = "I took a short walk and felt a little more grounded afterward.",
                ShareWithPartner = false
            },
            new JournalEntry
            {
                UserId = DevUserId,
                CreatedDate = now.AddDays(-1),
                Prompt = "What do you need more of this week?",
                Content = "More rest, more water, and asking for help sooner instead of pushing through.",
                ShareWithPartner = true
            }
        );

        db.SaveChanges();
    }

    public static void EnsureDevMoodEntries(FlourishDbContext db)
    {
        // Ensure 14 entries across 14 distinct days for the dev user.
        var existingDates = db.MoodEntries.AsNoTracking()
            .Where(m => m.UserId == DevUserId)
            .Select(m => m.Date)
            .ToHashSet(StringComparer.Ordinal);

        var toAdd = new List<MoodEntry>();
        var today = DateTime.UtcNow.Date;
        for (var i = 0; i < 14; i++)
        {
            var day = today.AddDays(-i);
            var date = day.ToString("yyyy-MM-dd");
            if (existingDates.Contains(date))
                continue;

            // A simple repeating pattern so charts look interesting.
            var moodValue = 45 + (i * 4 % 45); // 45..89
            var label = moodValue switch
            {
                >= 80 => "Great",
                >= 65 => "Good",
                >= 50 => "Okay",
                _ => "Low"
            };

            toAdd.Add(new MoodEntry
            {
                UserId = DevUserId,
                Date = date,
                Time = "09:00",
                MoodValue = moodValue,
                MoodLabel = label
            });
        }

        if (toAdd.Count == 0) return;
        db.MoodEntries.AddRange(toAdd);
        db.SaveChanges();
    }

    private static HashSet<string> GetDevAffirmationTexts() => new(StringComparer.Ordinal)
    {
        "I am doing my best, and that is enough.",
        "Small steps still move me forward.",
        "I can handle what comes next.",
        "I give myself permission to rest.",
        "My feelings are valid, and they will pass.",
        "I am learning as I go.",
        "I deserve support and kindness.",
        "I trust myself to make good decisions.",
        "I can start again at any moment.",
        "Today, I will focus on what I can control."
    };
}
