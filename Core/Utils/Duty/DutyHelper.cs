namespace Core.Utils.Duty;

public class DutyHelper
{
    /*
    public async Task DeleteOldDutiesAsync()
    {
        var dutiesToDelete = await _dutyDal.GetAllAsync(d => d.IsDeleted && d.DeletedAt <= DateTime.UtcNow.AddDays(-30));

        foreach (var duty in dutiesToDelete)
        {
            await _dutyDal.DeleteAsync(duty);
        }
    }*/
}