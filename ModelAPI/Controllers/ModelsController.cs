using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ModelAPI.Data;
using ModelAPI.Models;
using ModelAPI.Models.ModelDTO;

namespace ModelAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ModelsController : ControllerBase
{
	private readonly DataContext _context;

	public ModelsController(DataContext context)
	{
		_context = context;
	}


	[HttpGet("BaseData")]
	public async Task<ActionResult<List<ModelDto>>> GetModels()
	{
		var dbModel = await _context.Models.ToListAsync();
		if (dbModel == null) return BadRequest("Could not find any models");

		foreach (var model in dbModel)
			_context.Entry(model)
				.Collection(m => m.Jobs)
				.Load();

		return Ok(dbModel.Adapt<List<ModelDto>>());
	}


	[HttpGet("{modelId}")]
	public async Task<ActionResult<Model>> GetModel(long modelId)
	{
		// get model from database
		var dbModel = await _context.Models.FindAsync(modelId);

		if (dbModel == null) return NotFound("Could not find Model");

		_context.Entry(dbModel)
			.Collection(m => m.Jobs)
			.Load();

		_context.Entry(dbModel)
			.Collection(m => m.Expenses)
			.Load();

		return Ok(dbModel);
	}

	[HttpPost]
	public async Task<ActionResult<ModelDto>> PostModel(ModelDto createModel)
	{
		// use Mapster to map createModel to a Model
		var model = createModel.Adapt<Model>();

		// add model to database and save changes
		_context.Models.Add(model);
		await _context.SaveChangesAsync();

		// return model as a ModelDto
		return Ok(model.Adapt<ModelDto>());
	}


	[HttpDelete("{modelId}")]
	public async Task<ActionResult<Model>> DeleteModel(long modelId)
	{
		// get model from database
		var dbModel = await _context.Models.FindAsync(modelId);
		if (dbModel == null) return NotFound("Model not found");

		// delete model from database
		_context.Models.Remove(dbModel);


		_context.Entry(dbModel).State = EntityState.Deleted;

		// save changes
		await _context.SaveChangesAsync();

		// return the updated list of models
		return Ok(await _context.Models.ToListAsync());
	}


	[HttpPut("BaseData/{modelId}")]
	public async Task<ActionResult<ModelDto>> PutModel(long modelId, ModelDto modelDto)
	{
		// get model from database
		var dbModel = await _context.Models.FindAsync(modelId);
		if (dbModel == null) return NotFound("Could not find Model");

		// update model to database using mapster adapt
		var model = modelDto.Adapt(dbModel);
		_context.Models.Update(model);

		// save changes
		await _context.SaveChangesAsync();

		// return updated modelDto using mapster adapt
		return Ok(model.Adapt<ModelDto>());
	}


	private bool ModelExists(long id)
	{
		return _context.Models.Any(e => e.Id == id);
	}
}