using KASHOP.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Utilites
{
    public class UserSeedData : ISeedData
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserSeedData(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task DataSeed()
        {
            if (!await _userManager.Users.AnyAsync())
            {
                var user1 = new ApplicationUser
                {
                    UserName = "Hmood",
                    Email = "Hmood@gmail",
                    FullName = "Hmood Abu Omar",
                    EmailConfirmed = true,
                };
                var user2 = new ApplicationUser
                {
                    UserName = "abd",
                    Email = "abd@gmail",
                    FullName = "abd edaily",
                    EmailConfirmed = true,
                };
                var user3 = new ApplicationUser
                {
                    UserName = "saeed",
                    Email = "saedd@gmail",
                    FullName = "Saeed dert",
                    EmailConfirmed = true,
                };
                await _userManager.CreateAsync(user1, "Pass@1122");
                await _userManager.CreateAsync(user2, "Pass@1122");
                await _userManager.CreateAsync(user3, "Pass@1122");

                await _userManager.AddToRoleAsync(user1, "SuperAdmin");
                await _userManager.AddToRoleAsync(user2, "Admin");
                await _userManager.AddToRoleAsync(user3, "User");
            }

        }
    }
}
