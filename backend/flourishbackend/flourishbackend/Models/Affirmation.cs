using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Flourish.Models
{
    public class Affirmation
    {
        [Key]
        public Guid AffirmationId { get; set; } = Guid.NewGuid();

        [Required]
        public string Text { get; set; } = string.Empty;
    }
}
