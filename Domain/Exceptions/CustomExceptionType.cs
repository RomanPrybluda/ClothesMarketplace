namespace Domain
{
    public enum CustomExceptionType
    {
        InternalError = 1,
        NoContent = 2,
        NotFound = 3,
        IsAlreadyExists = 4,
        UserIsAlreadyExists = 5,
        InvalidInputData = 6,
        ProductAlreadyExists = 7,
        InvalidData = 8,
        PasswordsDoNotMatch = 11,
        FaildToAddRoleToUser = 12
    }
}
