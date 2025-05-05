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

            // Priority-based scoring with nested conditions
            if (Priority == 5)
            {
                if (IsCompleted)
                {
                    score += 100;
                    if (Status == "completed" && (DateTime.UtcNow - CreatedAt).TotalDays < 1)
                    {
                        score += 50;
                        if (!string.IsNullOrEmpty(Description) && Description.Length > 100)
                        {
                            score += 25;
                        }
                        else if (!string.IsNullOrEmpty(Description) && Description.Length > 50)
                        {
                            score += 10;
                        }
                    }
                    else if (Status == "completed" && (DateTime.UtcNow - CreatedAt).TotalDays < 7)
                    {
                        score += 30;
                        if (!string.IsNullOrEmpty(Description))
                        {
                            if (Description.Contains("urgent") || Description.Contains("important"))
                            {
                                score += 20;
                                if (Title.Length < 50)
                                {
                                    score += 5;
                                }
                            }
                        }
                    }
                }
                else
                {
                    score += 75;
                    if (Status == "in-progress")
                    {
                        score += 25;
                        if ((DateTime.UtcNow - CreatedAt).TotalHours < 24)
                        {
                            score += 15;
                            if (!string.IsNullOrEmpty(Description))
                            {
                                if (Description.Contains("blocking") || Description.Contains("critical"))
                                {
                                    score += 35;
                                }
                            }
                        }
                    }
                }
            }
            else if (Priority == 4)
            {
                if (IsCompleted)
                {
                    score += 80;
                    if (!string.IsNullOrEmpty(Description))
                    {
                        if (Description.Contains("bug") || Description.Contains("fix"))
                        {
                            score += 30;
                            if (Status == "completed" && (DateTime.UtcNow - CreatedAt).TotalDays < 3)
                            {
                                score += 20;
                            }
                        }
                    }
                }
                else
                {
                    score += 60;
                    if (Status == "in-progress" && (DateTime.UtcNow - CreatedAt).TotalDays < 5)
                    {
                        score += 25;
                        if (!string.IsNullOrEmpty(Title) && Title.Length < 30)
                        {
                            score += 15;
                        }
                    }
                }
            }
            else if (Priority == 3)
            {
                score = IsCompleted ? 60 : 40;
                if (!string.IsNullOrEmpty(Description))
                {
                    if (Description.Contains("enhancement") || Description.Contains("feature"))
                    {
                        score += IsCompleted ? 25 : 15;
                        if (Status == "in-progress")
                        {
                            score += 10;
                        }
                    }
                }
            }
            else if (Priority == 2)
            {
                score = IsCompleted ? 40 : 20;
            }
            else
            {
                score = IsCompleted ? 20 : 10;
            }

            return score;
        }
    }
}
