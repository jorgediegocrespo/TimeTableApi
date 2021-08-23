﻿using TimeTable.Application.Contracts.Models;
using TimeTable.Application.Mappers.Base;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Mappers
{
    public class VacationDayMapper : IMapper<VacationDay, VacationDayEntity>
    {
        public VacationDayEntity Map(VacationDay businessModel)
        {
            return new VacationDayEntity()
            {
                Id = businessModel.Id,
                Year = businessModel.Year,
                VacationDays = businessModel.VacationDays,
            };
        }

        public VacationDay Map(VacationDayEntity entity)
        {
            return new VacationDay(); //TODO
        }
    }
}
