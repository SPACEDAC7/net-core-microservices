using Actio.Common.Exceptions;
using Actio.Services.Activities.Domain.Models;
using Actio.Services.Activities.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Actio.Services.Activities.Services
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository activityRepository;
        private readonly ICategoryRepository categoryRepository;

        public ActivityService(IActivityRepository activityRepository, ICategoryRepository categoryRepository)
        {
            this.activityRepository = activityRepository;
            this.categoryRepository = categoryRepository;
        }

        public async Task AddAsync(Guid id, Guid userId, string category, string name, string description, DateTime createdAt)
        {
            Console.WriteLine($"Activity Service: Data - {category} - {name} - {userId} - {id} -{description}");

            var activityCategory = await this.categoryRepository.GetAsync(category);

            Console.WriteLine($"ActivityCategory -> {activityCategory}");

            if (activityCategory == null)
            {
                throw new ActioException("category_not_found", $"Category: {category} was not found.");
            }
            var activity = new Activity(id, name, activityCategory, description, userId, createdAt);
            Console.WriteLine(activity.ToString());
            await this.activityRepository.AddAsync(activity);
        }
    }
}
