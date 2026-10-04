using System;
using ruru_api.src.DTOs.Workouts;

namespace ruru_api.src.services;

public interface IWorkoutService
{
    Task <WorkoutResponse> CreateAsync(
        Guid userId,
        CreateWorkoutRequest request
    );
    Task<WorkoutResponse?> GetByIdAsync(Guid Id);
    Task<List<WorkoutResponse>> GetByUserIdAsync(Guid userId);
}
