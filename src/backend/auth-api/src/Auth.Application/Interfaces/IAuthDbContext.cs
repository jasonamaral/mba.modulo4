using Auth.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Auth.Application.Interfaces;

public interface IAuthDbContext
{
    DbSet<ApplicationUser> Users { get; set; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
} 