using DevTechTest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace DevTechTest.Data
{
    public class Repo: IRepo
    {
        private readonly JobDbContext _dbContext;

        public Repo(JobDbContext dbContext)
        {
            _dbContext = dbContext;
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

        public IEnumerable<JobNote> GetNotesForJob(int jobId)
        {
            IEnumerable<JobNote> notes = _dbContext.JobNotes.Where(note => note.JobId == jobId);
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
