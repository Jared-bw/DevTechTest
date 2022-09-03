using Microsoft.AspNetCore.Mvc;
using DevTechTest.Models;
using DevTechTest.Data;
using DevTechTest.DTO;
using Microsoft.EntityFrameworkCore;

namespace DevTechTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JobController : Controller
    {
        private readonly IRepo _repo;
        public JobController(IRepo repo)
        {
            _repo = repo;
        }


        /// <summary>
        /// Retrieves a job record from the database with the supplied id
        /// </summary>
        /// <param name="id"></param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        [HttpGet("ViewJob/{id}")]
        public async Task<ActionResult<JobDTO>> GetJobByIdAsync(int id)
        {
            JobDTO? dto = await _repo.GetJobAndClientAsync(id);
            if (dto == null)
                return NotFound();

            return Ok(dto);
        }

        /// <summary>
        /// Updates the status for a given job
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="status"></param>
        /// <returns>JobDTO of updated object</returns>
        [HttpPut("SetJobStatus")]
        [Produces("application/json")]
        public async Task<ActionResult<JobDTO>> SetJobStatusAsync(int jobId, JobStatus status)
        {
            Job? job = await _repo.GetJobByIdAsync(jobId);
            if (job == null)
                return NotFound();
            job.Status = status;
            await _repo.SaveChangesAsync();
            JobDTO? dto = await _repo.GetJobAndClientAsync(jobId);
            return Ok(dto);
        }


        /// <summary>
        /// Retrieves all the notes for a particular job
        /// </summary>
        /// <param name="jobId"></param>
        [HttpGet("GetNotes/{jobId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<JobNote>>> GetNotesForJobAsync(int jobId)
        {
            Job? j = await _repo.GetJobByIdAsync(jobId);
            if (j == null)
                return BadRequest();
            IEnumerable<JobNote> notes = await _repo.GetNotesForJobAsync(jobId);
            return Ok(notes);
        }


        /// <summary>
        /// Adds a note to the database for the given jobId. If the job doesn't exist then
        /// the controller returns BadRequest.
        /// </summary>
        /// <param name="note"></param>
        /// <returns>The newly added note</returns>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [HttpPost("AddNote")]
        public async Task<ActionResult<JobNote>> AddJobNoteAsync(NoteInputDTO note)
        {
            Job? j = await _repo.GetJobByIdAsync(note.JobId);
            if (j == null)
                return BadRequest();
            JobNote newNote = await _repo.AddJobNoteAsync(
                new JobNote { JobId = note.JobId, Note = note.Note });
            return Accepted(newNote);
            // I would use CreatedAtAction(), however I don't have an appropriate endpoint for it.
            // I'd make it, but I'm unfortunately very limited on time, too much life to live!
        }

        /// <summary>
        /// Updates a note for the given noteId, else returns BadRequest 400
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="updatedNote"></param>
        /// <returns>Updated note</returns>
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Produces("application/json")]
        [HttpPut("UpdateNote/{noteId}")]
        public async Task<ActionResult<JobNote>> UpdateNote(int noteId, string updatedNote)
        {
            JobNote? note = await _repo.GetJobNoteAsync(noteId);
            if (note == null)
                return BadRequest();

            note.Note = updatedNote;
            await _repo.SaveChangesAsync();
            return Accepted(note);
        }

        /// <summary>
        /// Gets the job records from the database, sorted and filtered according
        /// to the input strings.
        /// If no sorting input, then it will sort by Client emails alphabetically
        /// 
        /// </summary>
        /// <param name="sortOrder"></param>
        /// <param name="status"></param>
        [HttpGet("GetJobsFilteredAndSorted")]
        public async Task<ActionResult<JobDTO>> GetJobsFilteredAndSorted(string? sortOrder, JobStatus? status)
        {
            // I think this implementation is terrible, but I didn't have time to debug the cleaner way I wanted to do it.
            IQueryable<JobDTO> query;
            if (status != null)
            {
                query = _repo.GetAllJobsQueryFiltered(status);
            } else
            {
                query = _repo.GetAllJobsQuery();
            }

            sortOrder = String.IsNullOrWhiteSpace(sortOrder) ? "" : sortOrder.ToLower();
            switch (sortOrder)
            {
                case "date":
                    query = query.OrderBy(j => j.DateCreated);
                    break;
                case "date_desc":
                    query = query.OrderByDescending(j => j.DateCreated);
                    break;
                case "client_name":
                    query = query.OrderBy(j => j.ClientName);
                    break;
                case "client_name_desc":
                    query = query.OrderByDescending(j => j.ClientName);
                    break;
                default:
                    query = query.OrderBy(j => j.ClientEmail);
                    break;
            }

            return Ok(await query.ToListAsync());
        }
    }
}
