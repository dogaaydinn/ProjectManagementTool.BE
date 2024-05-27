using AutoMapper;
using Business.Services.DutyManagement.Abstract;
using Core.ExceptionHandling;
using Core.Services.Messages;
using Core.Services.Result;
using Core.Utils.IoC;
using DataAccess.Repositories.Abstract.TaskManagement;
using Domain.DTOs.DutyManagement;
using Domain.Entities.DutyManagement;

namespace Business.Services.DutyManagement.Concrete;

public class DutyManager : IDutyService
{
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    private readonly IDutyDal _dutyDal = ServiceTool.GetService<IDutyDal>()!;
    
    #region GetAll
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetAllAsync()
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var duties = await _dutyDal.GetAllAsync(x => x.IsDeleted == false);
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-123456", e.Message));
        }

        return result;
    }
    #endregion

    #region GetById
    public async Task<ServiceObjectResult<DutyGetDto?>> GetByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<DutyGetDto?>();

        try
        {
            var duty = await _dutyDal.GetAsync(p => p.Id == id && p.IsDeleted == false);
            var dutyDto = _mapper.Map<DutyGetDto>(duty);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-102348", e.Message));
        }
        
        return result;

    }
    #endregion

    #region GetByUserId
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByUserIdAsync(Guid userId)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var duties = await _dutyDal.GetAllAsync(d => d.AssignedUsers != null && d.AssignedUsers.Any(u => u.Id == userId) && d.IsDeleted == false);
            var dutyDtos = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDtos);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-328576", e.Message));
        }

        return result;
    }
    #endregion

    #region GetByTitle
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByTitleAsync(string title)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var duties = await _dutyDal.GetAllAsync(x => x.Title == title && x.IsDeleted == false);
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-212122", e.Message));
        }

        return result;
    }
    #endregion

    #region GetByProjectId
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByProjectIdAsync(Guid projectId)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var duties = await _dutyDal.GetAllAsync(x => x.ProjectId == projectId && x.IsDeleted == false);
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-846372", e.Message));
        }

        return result;
    }
    #endregion

    #region GetByStatus
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByStatusAsync(int status)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var statusEnum = (Core.Constants.Duty.DutyStatus)status;
            var duties = await _dutyDal.GetAllAsync(x => x.Status == statusEnum && x.IsDeleted == false);
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-123456", e.Message));
        }

        return result;
    }
    #endregion
   
    #region GetByPriority
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByPriorityAsync(int priority)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var priorityEnum = (Core.Constants.Duty.Priority)priority;
            var duties = await _dutyDal.GetAllAsync(x => x.Priority == priorityEnum && x.IsDeleted == false);
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-567745", e.Message));
        }

        return result;
    }
    #endregion
    
    #region GetByReporterId
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByReporterIdAsync(Guid reporterId)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var duties = await _dutyDal.GetAllAsync(x => x.ReporterId == reporterId && x.IsDeleted == false);
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-676869", e.Message));
        }

        return result;
    }
    #endregion

    #region GetByAssigneeId
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByAssigneeIdAsync(Guid assigneeId)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var duties = await _dutyDal.GetAllAsync(x => x.AssignedUsers != null && x.AssignedUsers.Any(u => u.Id == assigneeId) && x.IsDeleted == false);
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-343536", e.Message));
        }

        return result;
    }

    #endregion

    #region GetByDutyType
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByDutyTypeAsync(int dutyType)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var duties = await _dutyDal.GetAllAsync(x => x.DutyType.ToString() == dutyType.ToString() && x.IsDeleted == false);
            var dutyDto = _mapper.Map<List<DutyGetDto?>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-858990", e.Message));
        }

        return result;
    }
    #endregion

    #region GetByLabelId
    public async Task<ServiceCollectionResult<DutyGetDto?>> GetByLabelIdAsync(Guid labelId)
    {
        var result = new ServiceCollectionResult<DutyGetDto?>();

        try
        {
            var duties = await _dutyDal.GetAllAsync(x => x.Labels != null && x.Labels.Any(l => l.Id == labelId) && x.IsDeleted == false);
            var dutyDto = _mapper.Map<List<DutyGetDto>>(duties);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-123456", e.Message));
        }

        return result;
    }
    #endregion

    #region Update
    public async Task<ServiceObjectResult<DutyGetDto?>> UpdateAsync(DutyUpdateDto dutyUpdateDto)
    {
        var result = new ServiceObjectResult<DutyGetDto?>();

        try
        {
            var duty = await _dutyDal.GetAsync(u => u.Id == dutyUpdateDto.Id && u.IsDeleted == false);

            if (duty == null)
            {
                result.Fail(new ErrorMessage("DUTY-123456", "Duty not found"));
                return result;
            }

            _mapper.Map(dutyUpdateDto, duty);
            await _dutyDal.UpdateAsync(duty);
            var dutyDto = _mapper.Map<DutyGetDto>(duty);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-123456", e.Message));
        }

        return result;
    }
    #endregion
    
    #region Create
    public async Task<ServiceObjectResult<DutyGetDto>> CreateAsync(DutyCreateDto dutyCreateDto)
    {
        var result = new ServiceObjectResult<DutyGetDto>();

        try
        {
            var duty = _mapper.Map<Duty>(dutyCreateDto);
            await _dutyDal.AddAsync(duty);
            result.SetData(_mapper.Map<DutyGetDto>(duty));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-453356", e.Message));
        }
        return result;
    }
    #endregion
    
    #region DeleteById
    public async Task<ServiceObjectResult<DutyGetDto?>> DeleteByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<DutyGetDto?>();

        try
        {
            var duty = await _dutyDal.GetAsync(u => u.Id == id && u.IsDeleted == false);

            if (duty == null)
            {
                result.Fail(new ErrorMessage("DUTY-978563", "Duty not found"));
                return result;
            }

            await _dutyDal.SoftDeleteAsync(duty);
            var dutyDto = _mapper.Map<DutyGetDto>(duty);
            result.SetData(dutyDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("DUTY-987544", e.Message));
        }

        return result;
    }
    #endregion
}