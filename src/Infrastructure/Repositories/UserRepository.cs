using Application.Common.Interfaces.Repositories;
using Domain.Common.Pagination;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Repositories
{
    public class UserRepository(AppDbContext context, UserManager<User> userManager) : IUserRepository
    {
        public async Task<(string email, string? token)> AddNewUserAsync(User user, string password)
        {
            user.EmailConfirmed = false;

            //create new user
            var result = await userManager.CreateAsync(user, password);


            if (!result.Succeeded)
            {
                return (user.Email, null);
            }


            //generate token
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

            if (token is null)
            {
                return (user.Email, null);
            }

            return (user.Email, token);

        }

        public async Task<bool> ConfirmUserAsync(User user, string token)
        {
            var result = await userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded;
        }

        public async Task<IEnumerable<User>> GetExpiredUnConfirmedUsersAsync()
        {
            var ExpiredUsers = await context.Users.Where(u => !u.EmailConfirmed
            && u.CreatedAt.AddHours(24) >= DateTime.UtcNow).ToListAsync();

            return ExpiredUsers;
        }

        public async Task<User?> GetConfirmedUserByEmailAsync(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.EmailConfirmed && !u.IsDeleted);
            return user;
        }

        public async Task<User?> GetConfirmedUserByIdAsync(string id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id && u.EmailConfirmed && !u.IsDeleted);
            return user;
        }

        public async Task<User?> GetConfirmedByEmailAndPasswordAsync(string email, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email && u.EmailConfirmed && !u.IsDeleted);
            if (user is null)
                return null;

            var isPasswordCorrect = await userManager.CheckPasswordAsync(user, password);
            return isPasswordCorrect ? user : null;
        }

        public async Task<bool> IsExistByEmailAsync(string email)
        {
            var isExist = await context.Users.AnyAsync(u => u.Email == email && !u.IsDeleted);
            return isExist;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
            return user;
        }

        public async Task<User?> GetByIdAsync(string userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted);
            return user;
        }

        public async Task RemoveUnConfirmedUsersAsync()
        {
            var expiredUnConfirmedUsers = await GetExpiredUnConfirmedUsersAsync();

            if (!expiredUnConfirmedUsers.Any())
                return;

            context.RemoveRange(expiredUnConfirmedUsers);


            return;
        }

        public void UpdateUser(User user)
        {
            context.Users.Update(user);
        }

        public async Task<bool> UpdatePasswordAsync(User user, string newPassword)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            if (string.IsNullOrEmpty(token))
                return false;

            var resetPasswordResult = await userManager.ResetPasswordAsync(user, token, newPassword);
            return resetPasswordResult.Succeeded;
        }

        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            return token;
        }

        public async Task<bool> ResetPasswordAsync(User user, string token, string newPassword)
        {
            var resetPasswordResult = await userManager.ResetPasswordAsync(user, token, newPassword);
            return resetPasswordResult.Succeeded;
        }

        public async Task<string> GenerateChangeEmailTokenAsync(User user, string newEmail)
        {
            var token = await userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            return token;
        }

        public async Task<bool> ChangeEmailAsync(User user, string token, string newEmail)
        {
            var changeEmailResult = await userManager.ChangeEmailAsync(user, newEmail, token);
            return changeEmailResult.Succeeded;
        }

        public async Task<bool> DeleteAsync(User user)
        {
            //update user
            user.IsDeleted = true;
            user.DeletedAt = DateTime.UtcNow;

            var result = await userManager.UpdateAsync(user);

            return result.Succeeded;
        }

        public async Task<PaginationResult<User>> GetAllUsers(int pageNumber,int pageSize)
        {
            var query = context.Users.Where(u => !u.IsDeleted);

            var totalCount = await query.CountAsync();
            var users = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PaginationResult<User>(users, totalCount, pageNumber, pageSize);
        }
    }
}
