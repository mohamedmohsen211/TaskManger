using TaskManger.API.Data;
using TaskManger.API.Models;

namespace TaskManger.API.Services
{
    public class LogService
    {
        private readonly AppDpContext _context;
        public LogService(AppDpContext context)
        {
            _context = context;
        }
        public async Task LogAsync(Guid? userId, string action, string ipAddress)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "UserId cannot be null.");
            }

            var log = new Log
            {
                UserId = userId,
                Action = action,
                IpAddress = ipAddress,
            };
            _context.Logs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}
