﻿using System.ComponentModel.DataAnnotations;
using ModelAPI.Models.ModelDTO;

namespace ModelAPI.Models.JobDTO;

public class JobWithModelsDto
{
	public long Id { get; set; }
	public string? Customer { get; set; }
	public DateTimeOffset StartDate { get; set; }
	public int Days { get; set; }

	[MaxLength(128)] public string? Location { get; set; }

	[MaxLength(2000)] public string? Comments { get; set; }

	public ICollection<ModelDto> Models { get; set; } = new List<ModelDto>();
}