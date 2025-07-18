using BFF.Domain.DTOs;

namespace BFF.Application.Interfaces.Services;

public interface IDashboardService
{
    Task<DashboardAlunoDto> GetDashboardAlunoAsync(Guid userId);
    Task<DashboardAdminDto> GetDashboardAdminAsync();
} 