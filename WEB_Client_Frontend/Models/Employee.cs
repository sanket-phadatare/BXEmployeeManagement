using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WEB_Client_Frontend.Models
{
    public class Employee
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        [Required(ErrorMessage = "Name is required")]
        [MinLength(2, ErrorMessage = "Minimum 2 characters should be there in the name")]
        [MaxLength(20, ErrorMessage = "Maximum 20 characters in the name are allowed")]
        public string Name { get; set; }

        [JsonPropertyName("designation")]
        [Required]
        public string Designation { get; set; }

        [JsonPropertyName("dateOfJoining")]
        public DateTime DateOfJoining { get; set; } = DateTime.Today;

        [JsonPropertyName("salary")]
        public Decimal Salary { get; set; }

        [JsonPropertyName("gender")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public GenderType Gender { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        // Add the DateOfBirth property for Age calculation
        [JsonPropertyName("dateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        // Age will be calculated on the frontend
        [JsonPropertyName("age")]
        public int Age { get; set; }
    }
}
