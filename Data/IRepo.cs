using DevTechTest.Models;

namespace DevTechTest.Data
{
    public interface IRepo
    {
        Task<Job?> GetJobByIdAsync(int id);

        Task<Client?> GetClientByIdAsync(int id);

        Task SaveChangesAsync();

        IEnumerable<JobNote> GetNotesForJob(int jobId);

        Task<JobNote> AddJobNoteAsync(JobNote note);

        Task<JobNote?> GetJobNoteAsync(int jobNoteId);
    }
}
