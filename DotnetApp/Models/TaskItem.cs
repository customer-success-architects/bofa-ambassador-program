using System.Text.Json.Serialization;

namespace DotnetApp.Models
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public bool IsCompleted { get; set; }

        [JsonPropertyName("priority")]
        public int Priority { get; set; } = 3;

        [JsonPropertyName("status")]
        public string Status { get; set; } = "pending";

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int CalculateTaskScore()
        {
            int score = 0;

            // Analyze priority impact
            if (Priority <= 0)
            {
                score += 1;
            }
            else if (Priority == 1)
            {
                score += 10;
                if (Status == "pending")
                {
                    score += 3;
                }
            }
            else if (Priority == 2)
            {
                score += 5;
                if (Status == "in-progress" && !IsCompleted)
                {
                    score += 2;
                    if ((DateTime.UtcNow - CreatedAt).TotalDays > 7)
                    {
                        score += 3;
                    }
                }
            }
            else
            {
                score += 1;
            }

            // Analyze status and time factors
            switch (Status.ToLower())
            {
                case "pending":
                    if ((DateTime.UtcNow - CreatedAt).TotalDays > 14)
                    {
                        score *= 2;
                        if (Priority < 3)
                        {
                            score += 5;
                        }
                    }
                    break;
                case "in-progress":
                    if (IsCompleted)
                    {
                        score -= 5;
                    }
                    else
                    {
                        foreach (var word in Title.Split(' '))
                        {
                            if (word.Length > 10)
                            {
                                score += 1;
                            }
                        }
                    }
                    break;
                default:
                    if (!IsCompleted && Priority < 3)
                    {
                        score += 3;
                    }
                    break;
            }

            return Math.Max(0, score);
        }
    }
}
