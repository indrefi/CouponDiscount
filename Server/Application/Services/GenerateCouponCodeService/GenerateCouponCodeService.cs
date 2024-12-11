using Application.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Services.GenerateCouponCodeService
{
    public class GenerateCouponCodeService : IGenerateCouponCodeService
    {
        private readonly ILogger<GenerateCouponCodeService> _logger;

        public GenerateCouponCodeService(ILogger<GenerateCouponCodeService> logger)
        {
            _logger = logger;
        }

        public async Task<List<string>> GenerateCouponCodes(GenerateCouponCodeRequest generateCouponCodeRequest)
        {
            try
            {
                int numberOfTasks = generateCouponCodeRequest.CouponInstances;
                var tasks = new List<Task<string>>();

                for (int taskId = 1; taskId <= numberOfTasks; taskId++)
                {
                    var task = Task.Run(() => GenerateCode(generateCouponCodeRequest.CouponLength, taskId));
                    tasks.Add(task);
                }

                var results = await Task.WhenAll(tasks);

                return results.ToList();
            }
            catch(Exception ex)
            {
                _logger.LogError($"Error encountered in {this.GetType().Name} ", ex);

                return new List<string>();
            }            
        }

        private string GenerateCode(int maxLength, int taskId)
        {
            var seed = Guid.NewGuid();

            // xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx 8-4-4-4-12
            var sections = seed.ToString().Split('-');

            var section = sections.Last();
            var substringValue = maxLength == 8? section[..(section.Length - 4)] : section[..(section.Length - 5)];

            return substringValue;
        }
    }
}
