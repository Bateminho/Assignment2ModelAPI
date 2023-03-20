using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mapster;
using ModelAPI.Data;
using ModelAPI.Models;
using ModelAPI.Models.ExpenseDTO;
using ModelAPI.Models.JobDTO;

namespace ModelAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobsController : ControllerBase
    {
        private readonly DataContext _context;

        public JobsController(DataContext context)
        {
            _context = context;
        }

		[HttpGet("WithModelNames/{modelId}")]
		public async Task<ActionResult<JobWithModelNamesDto>> GetJobWithModelNames()
		{
			List<JobWithModelNamesDto> allJobsWithModelNames = new List<JobWithModelNamesDto>();

			// loop through all jobs in db and load all jobs with models
			foreach (var job in _context.Jobs)
			{
				// explicit loading of models for each job
				await _context.Entry(job).Collection(j => j.Models).LoadAsync();

				var aJobWithModelNames = job.Adapt<JobWithModelNamesDto>();
				aJobWithModelNames.ModelNames = new List<string>();
				foreach (var model in job.Models)
				{
					aJobWithModelNames.ModelNames.Add($"{model.FirstName} {model.LastName}");
				}
				allJobsWithModelNames.Add(aJobWithModelNames);
			}

			return Ok(allJobsWithModelNames);
		}


		[HttpGet("WithModel/{modelId}")]
		public async Task<ActionResult<List<JobWithModelsDto>>> GetJobsWithModels(long modelId)
		{
			// get all jobs from db
			var dbJobs = await _context.Jobs.ToListAsync();

			dbJobs.ForEach(async j => await _context.Entry(j)
				.Collection(j => j.Models)
				.LoadAsync());


			dbJobs = dbJobs
				.Where(j => j.Models.Any(m => m.Id == modelId))
				.ToList();

			return Ok(dbJobs.Adapt<List<JobWithModelsDto>>());
		}


		[HttpGet("WithExpenses/{jobId}")]
		public async Task<ActionResult<JobWithExpensesDto>> GetJobWithExpenses(long jobId)
		{
			var dbJob = await _context.Jobs.FindAsync(jobId);

			if (dbJob == null) { return NotFound("Did not find a job with id " + jobId); }

			_context.Entry(dbJob)
				.Collection(j => j.Expenses)
				.Load();
			_context.Entry(dbJob)
				.Collection(j => j.Models)
				.Load();

			// map the job to the jobWithExpensesdto and assign a new list of expenses to the dto list of expenses
			var jobWithExpenses = dbJob.Adapt<JobWithExpensesDto>();
			jobWithExpenses.Expenses = new List<ExpenseIdDto>();

			if (dbJob == null) { return NotFound("Did not find job with id " + jobId); }

			// add all expenses related to the job to the jobWithExpenses object
			foreach (var expense in dbJob.Expenses)
			{
				jobWithExpenses.Expenses.Add(expense.Adapt<ExpenseIdDto>());
			}

			return Ok(jobWithExpenses);
		}


		[HttpPost]
        public async Task<ActionResult<JobIdDto>> PostJob(JobNoIdDto jobCreate)
        {
            // add the expense to the database and save changes
            _context.Jobs.Add(jobCreate.Adapt<Job>());
            await _context.SaveChangesAsync();

            var dbJobs = await _context.Jobs.ToListAsync();

            return Accepted(dbJobs.Adapt<List<JobIdDto>>());
        }

        
        [HttpDelete("{jobId}")]
        public async Task<ActionResult<Job>> DeleteJob(long jobId)
        {
            var job = await _context.Jobs.FindAsync(jobId);
            if (job == null)
            {
                return NotFound("Did not find the job");
            }

            _context.Jobs.Remove(job);
            await _context.SaveChangesAsync();

            return Ok(await _context.Jobs.ToListAsync());
        }

        
        [HttpPut("{jobId}")]
        public async Task<ActionResult<Job>> PutJob(long jobId, JobUpdateDto jobUpdate)
        {
            // get model from database
            var dbJob = await _context.Jobs.FindAsync(jobId);
            if (dbJob == null) { return NotFound("Could not find Job"); }

            // update model in database using mapster adapt
            var job = jobUpdate.Adapt(dbJob);
            _context.Jobs.Update(job);
            
            // save changes
            await _context.SaveChangesAsync();

            // return updated job using mapster adapt
            return Ok(dbJob);
        }

        
        [HttpPut("{jobId}/AddModel/{modelId}")]
        public async Task<ActionResult<JobWithModelsDto>> AddModelToJob(long modelId, long jobId)
        {
            var dbModel = await _context.Models.FindAsync(modelId);
            if (dbModel == null) { return NotFound("Could not find Model"); }

            var dbJob = await _context.Jobs.FindAsync(jobId);
            if (dbJob == null) { return NotFound("Could not find Job"); }
            
            _context.Entry(dbJob)
                .Collection(j => j.Models)
                .Load();

            _context.Entry(dbModel)
             .Collection(m => m.Jobs)
             .Load();

            if (dbJob.Models.Contains(dbModel)) { return Conflict("Model already on Job"); }

            dbJob.Models.Add(dbModel);
            await _context.SaveChangesAsync();
            
            return Accepted(dbModel.Adapt<JobWithModelsDto>());
        }

        
        [HttpPut("{jobId}/RemoveModel/{modelId}")]
        public async Task<ActionResult<Job>> RemoveModelFromJob(long modelId, long jobId)
        {
            var dbJob = await _context.Jobs.FindAsync(jobId);
            
            if (dbJob == null) { return NotFound("Did not find job with id " + jobId); }
            
            _context.Entry(dbJob)
                .Collection(j => j.Models)
                .Load();

            var dbModel = await _context.Models.FindAsync(modelId);
            if (dbModel == null) { return NotFound("Did not find model with id " + modelId); }

            if (!dbJob.Models.Contains(dbModel)) { return BadRequest("Model not on job"); }

            dbJob.Models.Remove(dbModel);
            await _context.SaveChangesAsync();

            return Accepted(dbModel);

        }

        
        

    }
}
