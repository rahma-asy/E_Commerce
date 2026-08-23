using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace E_Commerce.Application.Comman
{
    //just holder no need for class featuers
    public sealed record Error(string Code, string Description, ErrorType ErrorType = ErrorType.Failure)
    {
        public static Error Failure(string code = "General.Failure", string description = "General Failure has Occurred")
        => new(code, description, ErrorType.Failure);
        public static Error Validation(string code = "General.Validation", string description = "General Validation Error has Occurred")
        => new(code, description, ErrorType.Validation);
        public static Error NotFound(string code = "General.NotFound", string description = "Resource NotFound")
        => new(code, description, ErrorType.NotFound);
        public static Error Conflict(string code = "General.Conflict", string description = "General Conflict has Occurred")
        => new(code, description, ErrorType.Conflict);
        public static Error Unauthorized(string code = "General.Unauthorized", string description = "Access Is Denied")
        => new(code, description, ErrorType.Unauthorized);
        public static Error Forbidden(string code = "General.Forbidden", string description = "This Operation Is Forbidden")
        => new(code, description, ErrorType.Forbidden);
        public static Error InvalidCredentials(string code = "General.InvalidCredentials", string description = "Provided Credentials Are Invalid")
         => new(code, description, ErrorType.InvalidCredentials);

    }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorType
    {
        Failure = 0,
        Validation = 1,
        NotFound = 2,
        Conflict = 3,
        Unauthorized = 4,
        Forbidden = 5,
        InvalidCredentials = 6
    }

}
