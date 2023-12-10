namespace PmsApi.Utilities;

using Microsoft.EntityFrameworkCore;
using Task = PmsApi.Models.Task;
public static class QueryHelper
{

    public static IQueryable<Task> ApplyIncludes(IQueryable<Task> query, string include)
    {
        var includes = include.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var item in includes)
        {
            switch (item)
            {

                case "user":
                    query = query.Include(x => x.AssignedUser);
                    break;
                case "project":

                    query = query.Include(t => t.Project);
                    break;
                case "attachments":

                    query = query.Include(t => t.TaskAttachments);
                    break;
            }

        }
        return query;
    }
}
