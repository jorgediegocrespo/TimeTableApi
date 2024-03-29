﻿using System;
using System.ComponentModel.DataAnnotations;

namespace TimeTable.Business.Models
{
    public class ReadingTimeRecord
    {
        public int Id { get; set; }
        public int PersonId { get; set; }
        public DateTimeOffset StartDateTime { get; set; }
        public DateTimeOffset? EndDateTime { get; set; }
        public byte[] RowVersion { get; set; }
    }

    public class CreatingTimeRecord
    {
        [Required]
        public DateTimeOffset StartDateTime { get; set; }

        public DateTimeOffset? EndDateTime { get; set; }
    }

    public class UpdatingTimeRecord : CreatingTimeRecord
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public byte[] RowVersion { get; set; }
    }
}
