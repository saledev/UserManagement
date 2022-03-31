namespace saledev.UserManagement;

public interface IRightValidationService
{
    Task<bool> HasRight(string rightName);
}
