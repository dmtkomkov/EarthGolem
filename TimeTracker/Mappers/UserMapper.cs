using Microsoft.AspNetCore.Identity;
using TimeTracker.Dtos.User;

namespace TimeTracker.Mappers;

public static class UserMapper {
    public static UserDto ToDto(this IdentityUser userModel) {
        return new UserDto() {
            UserId = userModel.Id,
            UserName = userModel.UserName
        };
    }
}