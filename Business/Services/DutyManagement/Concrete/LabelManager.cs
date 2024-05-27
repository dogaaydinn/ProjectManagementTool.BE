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

public class LabelManager : ILabelService
{
    private readonly IMapper _mapper = ServiceTool.GetService<IMapper>()!;
    private readonly ILabelDal _labelDal = ServiceTool.GetService<ILabelDal>()!;
    
    #region GetAll
    public async Task<ServiceCollectionResult<LabelGetDto?>> GetAllAsync()
    {
        var result = new ServiceCollectionResult<LabelGetDto?>();
        
        try
        {
            var labels = await _labelDal.GetAllAsync(x => x.IsDeleted == false);
            var labelDto = _mapper.Map<List<LabelGetDto>>(labels);
            result.SetData(labelDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("LABEL-121256", e.Message));
        }
        
        return result;
    }
    #endregion

    #region GetById

    public async Task<ServiceObjectResult<LabelGetDto?>> GetByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<LabelGetDto?>();

        try
        {
            var label = await _labelDal.GetAsync(p => p.Id == id && p.IsDeleted == false);
            var labelDto = _mapper.Map<LabelGetDto>(label);
            result.SetData(labelDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("LABEL-943056", e.Message));
        }
        
        return result;

    }


    #endregion

    #region GetLabelsByColor
    public async Task<ServiceCollectionResult<LabelGetDto?>> GetByColorAsync(string color)
    {
        var result = new ServiceCollectionResult<LabelGetDto?>();
        
        try
        {
            var labels = await _labelDal.GetAllAsync(x => x.Color == color && x.IsDeleted == false);
            var labelDto = _mapper.Map<List<LabelGetDto>>(labels);
            result.SetData(labelDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("LABEL-364855", e.Message));
        }
        
        return result;
    }

    #endregion

    #region Update
    public async Task<ServiceObjectResult<LabelGetDto?>> UpdateAsync(LabelUpdateDto labelUpdateDto)
    {
        var result = new ServiceObjectResult<LabelGetDto?>();

        try
        {
            var label = await _labelDal.GetAsync(u => u.Id == labelUpdateDto.Id && u.IsDeleted == false);

            if (label == null)
            {
                result.Fail(new ErrorMessage("LABEL-604964", "Label not found"));
                return result;
            }

            label = _mapper.Map(labelUpdateDto, label);
            await _labelDal.UpdateAsync(label);
            var labelDto = _mapper.Map<LabelGetDto>(label);
            result.SetData(labelDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("LABEL-435956", e.Message));
        }

        return result;
    }
    #endregion

    #region Create
    public async Task<ServiceObjectResult<LabelGetDto>> CreateAsync(LabelCreateDto labelCreateDto)
    {
        var result = new ServiceObjectResult<LabelGetDto>();

        try
        {
            var label = _mapper.Map<Label>(labelCreateDto);
            await _labelDal.AddAsync(label);
            result.SetData(_mapper.Map<LabelGetDto>(label));
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("LABEL-934885", e.Message));
        }
        return result;
    }
    #endregion
    
    #region DeleteById
    public async Task<ServiceObjectResult<LabelGetDto?>> DeleteByIdAsync(Guid id)
    {
        var result = new ServiceObjectResult<LabelGetDto?>();

        try
        {
            var label = await _labelDal.GetAsync(u => u.Id == id && u.IsDeleted == false);

            if (label == null)
            {
                result.Fail(new ErrorMessage("LABEL-604964", "Label not found"));
                return result;
            }

            await _labelDal.SoftDeleteAsync(label);
            var labelDto = _mapper.Map<LabelGetDto>(label);
            result.SetData(labelDto);
        }
        catch (ValidationException e)
        {
            result.Fail(e);
        }
        catch (Exception e)
        {
            result.Fail(new ErrorMessage("LABEL435956", e.Message));
        }

        return result;
    }
    #endregion
}