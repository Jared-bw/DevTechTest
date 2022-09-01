using DevTechTest.DTO;
using DevTechTest.Models;

namespace DevTechTest.Helper
{
    public class Helper
    {
        public static JobDTO GetJobDTO(Job job, Client client)
        {
            JobDTO dto = new JobDTO
            {
                JobId=job.Id,
                JobStatus = job.Status.ToString(),
                DateCreated=job.DateCreated.ToString(),
                ClientName=client.Name,
                ClientEmail=client.Email
            };
            return dto;
        }
    }
}
