using RS.Core.Domain;
using RS.Core.Service.DTOs;
using System;
using System.Collections.Generic;

namespace RS.Core.Testing.Arrange
{
    public static class UserDtos
    {
        public static List<User> InMemoryList()
        {
            var userData = new List<User>
            {
                new User{
                    ID = Guid.Parse("ae9569bf-3e20-4f6a-a930-a70066f8ceb8"),
                    Name="Test-Data-Name-1",
                    Email = "test1@test.com",
                    Phone="0000-000-0000",
                    IdentityUserID = Guid.Parse("b09fff82-a190-428c-bf39-f9e8bc36ed71"),
                    CreateBy=Guid.Parse("ae9569bf-3e20-4f6a-a930-a70066f8ceb8"),
                    CreateDT=new DateTime(1993,10,31),
                    IsDeleted=false
                },

                new User{
                    ID = Guid.Parse("debfce56-fcc3-4098-8935-c7e5656a5e48"),
                    Name="Test-Data-Name-2",
                    Email = "test2@test.com",
                    Phone="1111-111-1111",
                    IdentityUserID = Guid.Parse("32e1d2c7-eee0-4687-95c3-1259412ea81e"),
                    CreateBy=Guid.Parse("debfce56-fcc3-4098-8935-c7e5656a5e48"),
                    CreateDT=new DateTime(1903,03,19),
                    IsDeleted=false
                }
            };

            return userData;
        }
        public static UserAddDto RegisterDto()
        {
            UserAddDto model = new UserAddDto
            {
                Email = "test3@test.com",
                Name = "Test Data Name",
                Phone = "000-0000-000"
            };

            return model;
        }
        public static UserAddDto DuplicateUserDto()
        {
           UserAddDto model = new UserAddDto
            {
                Email = "test1@test.com"
            };

            return model;
        }
        public static UserUpdateDto UpdateDto()
        {
            UserUpdateDto model = new UserUpdateDto
            {
                ID = Guid.Parse("ae9569bf-3e20-4f6a-a930-a70066f8ceb8"),
                Name = "Test-Data-Name-1 UP",
                Phone = " 5555-555-5555"
            };

            return model;
        }
    }
}
