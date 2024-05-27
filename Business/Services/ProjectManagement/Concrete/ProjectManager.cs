using AutoMapper;
using Business.Services.ProjectManagement.Abstract;
using Core.ExceptionHandling;
using Core.Services.Messages;
using Core.Services.Result;
using Core.Utils.IoC;
using DataAccess.Repositories.Abstract.ProjectManagement;
using Domain.DTOs.ProjectManagement;
using Domain.Entities.ProjectManagement;

namespace Business.Services.ProjectManagement.Concrete;

public class ProjectManager : IProjectService
{
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    private readonly IProjectDal _projectDal = ServiceTool.GetService<IProjectDal>()!;
    
    #region GetAll
    public async Task<ServiceCollectionResult<ProjectGetDto?>> GetAllAsync()
    {
        var result = new ServiceCollectionResult<ProjectGetDto?>();
        
        try
        {
            var projects = await _projectDal.GetAllAsync(x => x.IsDeleted == false);
            var projectDto = _mapper.Map<List<ProjectGetDto>>(projects);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-789658", e.Message));
        }
        
        return result;
    }
    #endregion

    #region GetById
    public async Task<ServiceObjectResult<ProjectGetDto?>> GetByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<ProjectGetDto?>();

        try
        {
            var project = await _projectDal.GetAsync(p => p.Id == id && p.IsDeleted == false);
            var projectDto = _mapper.Map<ProjectGetDto>(project);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-567953", e.Message));
        }
        
        return result;

    }
    #endregion

    #region GetAllByManagerId
    public async Task<ServiceCollectionResult<ProjectGetDto?>> GetAllByManagerIdAsync(Guid id)
    {
        var result = new ServiceCollectionResult<ProjectGetDto?>();

        try
        {
            var projects = await _projectDal.GetAllAsync(p => p.ManagerId == id && p.IsDeleted == false);
            var projectDto = _mapper.Map<List<ProjectGetDto>>(projects);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-789658", e.Message));
        }

        return result;
    }
    #endregion

    #region GetProjectByStatus
    public async Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByStatusAsync(int status)
    {
        var result = new ServiceCollectionResult<ProjectGetDto?>();

        try
        {
            var projects = await _projectDal.GetAllAsync(p => (int)p.Status == status && p.IsDeleted == false);
            var projectDto = _mapper.Map<List<ProjectGetDto>>(projects);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-958473", e.Message));
        }

        return result;
    }
    #endregion

    #region GetProjectByPriority
    public async Task<ServiceObjectResult<ProjectGetDto>> GetProjectByPriorityAsync(int priority)
    {
        var result = new ServiceObjectResult<ProjectGetDto>();

        try
        {
            var project = await _projectDal.GetAsync(p => (int)p.Priority == priority && p.IsDeleted == false);
            var projectDto = _mapper.Map<ProjectGetDto>(project);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-112134", e.Message));
        }

        return result;
    }
    #endregion

    #region GetProjectByDutyId
    public async Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByDutyIdAsync(Guid id)
    {
        var result = new ServiceCollectionResult<ProjectGetDto?>();

        try
        {
            var projects = await _projectDal.GetAllAsync(p => p.Duties.Any(d => d.Id == id) && p.IsDeleted == false);
            var projectDto = _mapper.Map<List<ProjectGetDto>>(projects);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-958473", e.Message));
        }

        return result;
    }
    #endregion

    #region GetProjectByDueDate
    public async Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByDueDateAsync(DateTime dueDate)
    {
        var result = new ServiceCollectionResult<ProjectGetDto?>();

        try
        {
            var projects = await _projectDal.GetAllAsync(p => p.DueDate == dueDate && p.IsDeleted == false);
            var projectDto = _mapper.Map<List<ProjectGetDto>>(projects);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-958473", e.Message));
        }

        return result;
    }
    #endregion

    #region GetProjectByStartDate
    public async Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByStartDateAsync(DateTime startDate)
    {
        var result = new ServiceCollectionResult<ProjectGetDto?>();

        try
        {
            var projects = await _projectDal.GetAllAsync(p => p.StartDate == startDate && p.IsDeleted == false);
            var projectDto = _mapper.Map<List<ProjectGetDto>>(projects);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-958473", e.Message));
        }

        return result;
    }
    #endregion

    #region GetProjectByName
    public async Task<ServiceCollectionResult<ProjectGetDto?>> GetProjectByNameAsync(string name)
    {
        var result = new ServiceCollectionResult<ProjectGetDto?>();

        try
        {
            var projects = await _projectDal.GetAllAsync(p => p.Name == name && p.IsDeleted == false);
            var projectDto = _mapper.Map<List<ProjectGetDto>>(projects);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-958473", e.Message));
        }

        return result;
    }
    #endregion

    #region Update
    public async Task<ServiceObjectResult<ProjectGetDto?>> UpdateAsync(ProjectUpdateDto projectUpdateDto)
    {
        var result = new ServiceObjectResult<ProjectGetDto?>();

        try
        {
            var project = await _projectDal.GetAsync(u => u.Id == projectUpdateDto.Id && u.IsDeleted == false);

            if (project == null)
            {
                result.Fail(new ErrorMessage("PRJM-947833", "Project not found"));
                return result;
            }

            project = _mapper.Map(projectUpdateDto, project);
            await _projectDal.UpdateAsync(project!);
            result.SetData(_mapper.Map<ProjectGetDto>(project));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-238657", e.Message));
        }

        return result;
    }
    #endregion

    #region Create
    public async Task<ServiceObjectResult<ProjectGetDto>> CreateAsync(ProjectCreateDto projectCreateDto)
    {
        var result = new ServiceObjectResult<ProjectGetDto>();

        try
        {
            var project = _mapper.Map<Project>(projectCreateDto);
            await _projectDal.AddAsync(project);
            result.SetData(_mapper.Map<ProjectGetDto>(project));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-538408", e.Message));
        }
        return result;
    }
    #endregion
    
    #region DeleteById
    public async Task<ServiceObjectResult<ProjectGetDto?>> DeleteByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<ProjectGetDto?>();

        try
        {
            var project = await _projectDal.GetAsync(u => u.Id == id && u.IsDeleted == false);

            if (project == null)
            {
                result.Fail(new ErrorMessage("PRJM-947833", "Project not found"));
                return result;
            }

            await _projectDal.SoftDeleteAsync(project);
            var projectDto = _mapper.Map<ProjectGetDto>(project);
            result.SetData(projectDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("PRJM-238657", e.Message));
        }

        return result;
    }
    #endregion
}