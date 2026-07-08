using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetConfirmedUserByEmailAsync(string email);
        Task<User?> GetConfirmedUserByIdAsync(string id);
        Task<(string email, string? token)> AddNewUserAsync(User user, string password);
        Task<bool> ConfirmUserAsync(User user, string token);

        Task<bool> IsExistByEmailAsync(string email);

        Task<IEnumerable<User>> GetExpiredUnConfirmedUsersAsync();

        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(string userId);

        Task<User?> GetConfirmedByEmailAndPasswordAsync(string email, string password);
        Task RemoveUnConfirmedUsersAsync();

        void UpdateUser(User user);

        Task<bool> UpdatePasswordAsync(User user, string newPassword);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        Task<bool> ResetPasswordAsync(User user, string token, string newPassword);

        Task<string> GenerateChangeEmailTokenAsync(User user,string newEmail);
        Task<bool> ChangeEmailAsync(User user, string token, string newEmail);
    }   
}
