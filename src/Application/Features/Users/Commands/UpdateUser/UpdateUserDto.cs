using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Features.Users.Commands.UpdateUser
{
    public class UpdateUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly DateOfBirth { get; set; }
    }
}
