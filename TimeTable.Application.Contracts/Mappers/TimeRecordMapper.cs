﻿using TimeTable.Application.Contracts.Mappers.Base;
using TimeTable.Business.Models;
using TimeTable.DataAccess.Contracts.Entities;

namespace TimeTable.Application.Contracts.Mappers
{
    public class TimeRecordMapper : IMapper<BasicReadingTimeRecord, DetailedReadingTimeRecord, CreationTimeRecord, UpdatingTimeRecord, TimeRecordEntity>
    {
        public BasicReadingTimeRecord MapBasicReading(TimeRecordEntity entity)
        {
            return new BasicReadingTimeRecord()
            {
                Id = entity.Id,
                PersonId = entity.PersonId,
                StartDateTime = entity.StartDateTime,
                EndDateTime = entity.EndDateTime,
            };
        }
        public DetailedReadingTimeRecord MapDetailedReading(TimeRecordEntity entity)
        {
            return new DetailedReadingTimeRecord()
            {
                Id = entity.Id,
                PersonId = entity.PersonId,
                StartDateTime = entity.StartDateTime,
                EndDateTime = entity.EndDateTime,
            };
        }

        public TimeRecordEntity MapCreating(CreationTimeRecord businessModel)
        {
            return new TimeRecordEntity()
            {
                PersonId = businessModel.PersonId,
                StartDateTime = businessModel.StartDateTime,
                EndDateTime = businessModel.EndDateTime,
            };
        }


        public TimeRecordEntity MapUpdating(UpdatingTimeRecord businessModel)
        {
            return new TimeRecordEntity()
            {
                Id = businessModel.Id,
                PersonId = businessModel.PersonId,
                StartDateTime = businessModel.StartDateTime,
                EndDateTime = businessModel.EndDateTime,
            };
        }
    }
}
