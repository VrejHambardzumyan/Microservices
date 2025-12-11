using UserService.Domain.Entities;
using UserService.Domain.Repositories;
using UserService.Application.DTOs;

namespace UserService.Application.Services
{
    public interface IUserAccountService
    {
        Task<UserAccountDto> GetByIdAsync(Guid id);
        Task<UserAccountDto> CreateAsync(CreateUserAccountDto dto);
        Task<UserAccountDto> UpdateAsync(Guid id, UpdateUserAccountDto dto);
        Task<bool> DeleteAsync(Guid id);
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly IUserAccountRepository _repository;

        public UserAccountService(IUserAccountRepository repository)
        {
            _repository = repository;
        }

        public async Task<UserAccountDto> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            return new UserAccountDto
            {
                Id = entity.Id,
                Username = entity.Username,
                Email = entity.Email,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<UserAccountDto> CreateAsync(CreateUserAccountDto dto)
        {
            var entity = new UserAccount
            {
                Id = Guid.NewGuid(),
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                CreatedAt = DateTime.UtcNow
            };

            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();

            return new UserAccountDto
            {
                Id = entity.Id,
                Username = entity.Username,
                Email = entity.Email,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<UserAccountDto> UpdateAsync(Guid id, UpdateUserAccountDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return null;

            entity.Username = dto.Username ?? entity.Username;
            entity.Email = dto.Email ?? entity.Email;

            if (!string.IsNullOrWhiteSpace(dto.Password))
                entity.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _repository.SaveChangesAsync();

            return new UserAccountDto
            {
                Id = entity.Id,
                Username = entity.Username,
                Email = entity.Email,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            _repository.Delete(entity);
            await _repository.SaveChangesAsync();

            return true;
        }
    }
}