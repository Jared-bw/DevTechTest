using DevTechTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;
using DevTechTest.DTO;

namespace DevTechTest.Data
{
    public class Repo: IRepo
    {
        private readonly JobDbContext _dbContext;

        public Repo(JobDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        // Learning pulling from multiple relationships
        public async Task<JobDTO?> GetJobAndClientAsync(int jobId)
        {
            IQueryable<JobDTO> jobs = from j in _dbContext.Jobs
                       join c in _dbContext.Clients
                       on j.ClientId equals c.Id
                       where j.Id == jobId
                       select new JobDTO
                       {
                           JobId = j.Id,
                           JobStatus = j.Status.ToString(),
                           DateCreated = j.DateCreated.ToString(),
                           ClientName = c.Name,
                           ClientEmail = c.Email
                       };
            JobDTO? result = await jobs.FirstOrDefaultAsync();
            return result;
        }



        public async Task<Job?> GetJobByIdAsync(int id)
        {
            Job? job = (await _dbContext.Jobs.FindAsync(id));
            return job;
        }

        public async Task<Client?> GetClientByIdAsync(int id)
        {
            Client? client = await _dbContext.Clients.FindAsync(id);
            return client;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<JobNote>> GetNotesForJobAsync(int jobId)
        {
            var notesQuery = from n in _dbContext.JobNotes
                             where n.JobId == jobId
                             select n;
            List<JobNote> notes = await notesQuery.ToListAsync();
            return notes;
        }


        public async Task<JobNote> AddJobNoteAsync(JobNote note)
        {
            EntityEntry<JobNote> e = await _dbContext.JobNotes.AddAsync(note);
            JobNote newNote = e.Entity;
            await _dbContext.SaveChangesAsync();
            return newNote;
        }

        public async Task<JobNote?> GetJobNoteAsync(int jobNoteId)
        {
            JobNote? jobNote = await _dbContext.JobNotes.FirstOrDefaultAsync(n =>
                n.Id == jobNoteId);
            return jobNote;
        }

    }
}
