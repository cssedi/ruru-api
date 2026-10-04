using System;
using ruru_api.src.DTOs.Workouts;
using ruru_api.src.model;
namespace ruru_api.src.repositories;


public interface IWorkoutRepository
{
        Task<Workout?> GetByIdAsync(Guid id);
        Task<List<Workout>> GetByUserIdAsync(Guid userId);
        Task<Workout> AddAsync(Guid userId, CreateWorkoutRequest request);
        Task SaveChangesAsync();
}
