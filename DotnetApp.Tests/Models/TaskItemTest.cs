using System;
using Xunit;
using DotnetApp.Models;

namespace DotnetApp.Models
{
	public class TaskItemTest
	{
		[Fact]
		public void CalculateTaskScore_ShouldReturnCorrectScore_ForPriority5CompletedRecently()
		{
			// Arrange
			var task = new TaskItem
			{
				Priority = 5,
				IsCompleted = true,
				Status = "completed",
				CreatedAt = DateTime.UtcNow.AddHours(-12),
				Description = new string('a', 101)
			};

			// Act
			var score = task.CalculateTaskScore();

			// Assert
			Assert.Equal(175, score); // 100 + 50 + 25
		}

		[Fact]
		public void CalculateTaskScore_ShouldReturnCorrectScore_ForPriority5InProgress()
		{
			// Arrange
			var task = new TaskItem
			{
				Priority = 5,
				IsCompleted = false,
				Status = "in-progress",
				CreatedAt = DateTime.UtcNow.AddHours(-6),
				Description = "This is a critical blocking issue."
			};

			// Act
			var score = task.CalculateTaskScore();

			// Assert
			Assert.Equal(150, score); // 75 + 25 + 15 + 35
		}

		[Fact]
		public void CalculateTaskScore_ShouldReturnCorrectScore_ForPriority4CompletedWithBugFix()
		{
			// Arrange
			var task = new TaskItem
			{
				Priority = 4,
				IsCompleted = true,
				Status = "completed",
				CreatedAt = DateTime.UtcNow.AddDays(-2),
				Description = "Fixing a critical bug."
			};

			// Act
			var score = task.CalculateTaskScore();

			// Assert
			Assert.Equal(130, score); // 80 + 30 + 20
		}

		[Fact]
		public void CalculateTaskScore_ShouldReturnCorrectScore_ForPriority3InProgressWithFeature()
		{
			// Arrange
			var task = new TaskItem
			{
				Priority = 3,
				IsCompleted = false,
				Status = "in-progress",
				Description = "Adding a new feature."
			};

			// Act
			var score = task.CalculateTaskScore();

			// Assert
			Assert.Equal(65, score); // 40 + 15 + 10
		}

		[Fact]
		public void CalculateTaskScore_ShouldReturnCorrectScore_ForPriority2NotCompleted()
		{
			// Arrange
			var task = new TaskItem
			{
				Priority = 2,
				IsCompleted = false
			};

			// Act
			var score = task.CalculateTaskScore();

			// Assert
			Assert.Equal(20, score);
		}

		[Fact]
		public void CalculateTaskScore_ShouldReturnCorrectScore_ForPriority1Completed()
		{
			// Arrange
			var task = new TaskItem
			{
				Priority = 1,
				IsCompleted = true
			};

			// Act
			var score = task.CalculateTaskScore();

			// Assert
			Assert.Equal(20, score);
		}

		[Fact]
		public void CalculateTaskScore_ShouldHandleNullDescription()
		{
			// Arrange
			var task = new TaskItem
			{
				Priority = 3,
				IsCompleted = true,
				Description = null
			};

			// Act
			var score = task.CalculateTaskScore();

			// Assert
			Assert.Equal(60, score);
		}

		[Fact]
		public void CalculateTaskScore_ShouldHandleEmptyTitle()
		{
			// Arrange
			var task = new TaskItem
			{
				Priority = 4,
				IsCompleted = false,
				Status = "in-progress",
				CreatedAt = DateTime.UtcNow.AddDays(-3),
				Title = string.Empty
			};

			// Act
			var score = task.CalculateTaskScore();

			// Assert
			Assert.Equal(85, score); // 60 + 25
		}
	}
}