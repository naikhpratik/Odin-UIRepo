﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Odin.Data.Core.Dtos
{
    public class OrderDto
    {
        public OrderDto()
        {
            Services = new Collection<ServiceDto>();
        }

        [Required]
        public string TrackingId { get; set; }
        public string ProgramName { get; set; }
        public string RelocationType { get; set; }
        public DateTime? EstimatedArrivalDate { get; set; }
        public DateTime? PreTripDate { get; set; }
        public DateTime? EstimatedDepartureDate { get; set; }
        public DateTime? TempHousingEndDate { get; set; }

        public string DestinationCity { get; set; }
        public string DestinationState { get; set; }
        public string DestinationZip { get; set; }
        public string DestinationCountry { get; set; }

        public string OriginCity { get; set; }
        public string OriginState { get; set; }
        public string OriginCountry { get; set; }
        public string OriginZip { get; set; }

        public string SpouseName { get; set; }

        public string RmcContact { get; set; }
        public string RmcContactEmail { get; set; }

        public float? DaysAuthorized { get; set; }

        public string SeCustNumb { get; set; }
        public string Rmc { get; set; }
        public string Client { get; set; }

        public bool TransfereeInviteEnabled { get; set; }
        public bool IsRush { get; set; }
        public bool IsVip { get; set; }

        public int ServiceFlag { get; set; }
        public bool IsInternational { get; set; }
        public bool IsAssignment { get; set; }

        public int? LeaseTerm { get; set; }

        public string BrokerFeeTypeSeValue { get; set; }
        public string DepositTypeSeValue { get; set; }

        public string SeCustStatus { get; set;  }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string ProgramNotes { get; set; }


        [Required]
        public TransfereeDto Transferee { get; set; }

        [Required]
        public ProgramManagerDto ProgramManager { get; set; }

        [Required]
        public ConsultantDto Consultant { get; set; }

        public ICollection<ServiceDto> Services { get; set; }

        public ICollection<AppointmentDto> Appointments { get; set; }
    }
}
