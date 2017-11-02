using Odin.Data.Core.Models;
using Odin.Helpers;
using Odin.ViewModels.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Odin.ViewModels.Orders.Transferee
{
    public class OrdersTransfereeViewModel
    {
        public OrdersTransfereeViewModel()
        {
            Children = new List<ChildViewModel>();
            Services = new List<ServiceViewModel>();
            PossibleServices = new List<ServiceTypeViewModel>();
            Rent = new RentViewModel();
        }

        public string Id { get; set; }

        public string DestinationCity { get; set; }
        public string DestinationState { get; set; }
        public string DestinationCountry { get; set; }

        public string OriginCity { get; set; }
        public string OriginState { get; set; }
        public string OriginCountry { get; set; }

        public bool IsRush { get; set; }
        public bool IsVip { get; set; }

        public DateTime? PreTripDate { get; set; }
        public string PreTripDateDisplay => DateHelper.GetViewFormat(PreTripDate);

        public int TempHousingDays { get; set; }
        public DateTime? TempHousingEndDate { get; set; }       
        public string TempHousingEndDateDisplay =>
            DateHelper.GetViewFormat(TempHousingEndDate);

        public DateTime? FinalArrivalDate { get; set; }
        public string FinalArrivalDateDisplay => DateHelper.GetViewFormat(FinalArrivalDate);

        public DateTime? HomeFindingDate { get; set; }
        public string HomeFindingDateDisplay => DateHelper.GetViewFormat(HomeFindingDate);

        public DateTime? WorkStartDate { get; set; }
        public string WorkStartDateDisplay => DateHelper.GetViewFormat(WorkStartDate);

        public string SpouseName { get; set; }
        public string SpouseVisaType { get; set; }

        public string Rmc { get; set; }
        public string RmcContact { get; set; }
        public string RmcContactEmail { get; set; }
        
        private IEnumerable<ChildViewModel> _children;
        public virtual IEnumerable<ChildViewModel> Children
        {
            get
            {
                return _children.Where(c => !c.Deleted).ToList();
            }
            set
            {
                _children = value;
            }
        }
        public string ChildrenEducationPreferences { get; set; }
        public string SchoolDistrict { get; set; }
        public string SchoolDistrictDisplay => (SchoolDistrict != null ? SchoolDistrict : string.Empty);

        private IEnumerable<PetViewModel> _pets;
        public virtual IEnumerable<PetViewModel> Pets
        {
            get
            {
                return _pets.Where(p => !p.Deleted).ToList();
            }
            set
            {
                _pets = value;
            }
        }

        public string PetNotes { get; set; }

        public IEnumerable<ServiceViewModel> Services { get; set; }
        public IEnumerable<ServiceTypeViewModel> PossibleServices { get; set; }

        public TransfereeViewModel Transferee { get; set; }

        public string ProgramManagerId { get; set; }
        public virtual Manager ProgramManager { get; set; }

        public string PhoneNumberDisplay => (ProgramManager != null && ProgramManager.PhoneNumber != null)
           ? DateHelper.GetViewFormat(ProgramManager.PhoneNumber)
           : String.Empty;
        public string SeContactUidDisplay => (ProgramManager != null && ProgramManager.SeContactUid != null)
           ? ProgramManager.SeContactUid.ToString()
           : String.Empty;

        public string SeCustNumb { get; set; }
        public string Client { get; set; }
        public string ClientFileNumber { get; set; }
        public string ClientFileNumberDisplay => ClientFileNumber ?? ClientFileNumber;
           

        public string RentId { get; set; }
        public RentViewModel Rent { get; set; }
        
        public string HousingBudgetDisplay => (Rent != null && Rent.HousingBudget.HasValue)
            ? DateHelper.GetViewFormat(Rent.HousingBudget)
            : String.Empty;

        public string NumberOfBathroomsDisplay => (Rent != null && Rent.NumberOfBathrooms != null)
           ? Rent.NumberOfBathrooms.Name
            : String.Empty;

        public string NumberOfBedroomsDisplay => (Rent != null && Rent.NumberOfBedrooms.HasValue)
            ? Rent.NumberOfBedrooms.Value.ToString()
                : String.Empty;

        public string SquareFootageDisplay => (Rent != null && Rent.SquareFootage.HasValue)
            ? string.Format("{0: #,###.00}", Rent.SquareFootage)
            : String.Empty;

        public string HousingTypeDisplay => (Rent != null && Rent.HousingType != null)
            ? Rent.HousingType.Name
            : String.Empty;

        public string TransportationTypeDisplay => (Rent != null && Rent.TransportationType != null)
            ? Rent.TransportationType.Name
            : String.Empty;

        public string IsFurnishedDisplay => (Rent != null && Rent.IsFurnished.HasValue)
            ? Rent.IsFurnished.Value ? "Yes" : "No"
            : String.Empty;

        public string MaxCommuteDisplay => (Rent != null && Rent.MaxCommute.HasValue)
            ? Rent.MaxCommute.Value.ToString()
            : String.Empty;

        public string HasParkingDisplay => (Rent != null && Rent.HasParking.HasValue)
            ? Rent.HasParking.Value ? "Yes" : "No"
            : String.Empty;

        public string HasACDisplay => (Rent != null && Rent.HasAC.HasValue)
            ? Rent.HasAC.Value ? "Yes" : "No"
            : String.Empty;

        public string HasExerciseRoomDisplay => (Rent != null && Rent.HasExerciseRoom.HasValue)
            ? Rent.HasExerciseRoom.Value ? "Yes" : "No"
            : String.Empty;

        public string CommentsDisplay => (Rent != null && !String.IsNullOrEmpty(Rent.Comments))
            ? Rent.Comments
            : String.Empty;

        public string AreaTypeDisplay => (Rent != null && Rent.AreaType != null)
            ? Rent.AreaType.Name
            : String.Empty;

        public string HasLaundryDisplay => (Rent != null && Rent.HasLaundry.HasValue)
            ? Rent.HasLaundry.Value ? "Yes" : "No"
            : String.Empty;

        public string NumberOfCarsOwnedDisplay => (Rent != null && Rent.NumberOfCarsOwned.HasValue)
            ? Rent.NumberOfCarsOwned.Value.ToString()
            : String.Empty;

        public IEnumerable<NumberOfBathroomsType> NumberOfBathrooms { get; set; }

        public IEnumerable YesNoDropDown
        {
            get
            {
                return new[]
                {
                    new {Value = "True", Text = "Yes"},
                    new {Value = "False", Text = "No"},
                };
            }
        }

        public IEnumerable<HousingType> HousingTypes { get; set; }

        public IEnumerable<AreaType> AreaTypes { get; set; }

        public IEnumerable<TransportationType> TransportationTypes { get; set; }

        public DepositType DepositType { get; set; }

        public int? LeaseTerm { get; set; }

        public BrokerFeeType BrokerFeeType { get; set; }

        public IEnumerable<BrokerFeeType> BrokerFeeTypes { get; set; }

        public byte? BrokerFeeTypeId { get; set; }

        public string BrokerFeeTypeDisplay => (BrokerFeeType != null)
            ? BrokerFeeType.Name
            : String.Empty;

        public int? LengthOfAssignment { get; set; }

        public IEnumerable<DepositType> DepositTypes { get; set; }

        public byte? DepositTypeId { get; set; }

        public string DepositTypeDisplay => (DepositType != null)
            ? DepositType.Name
            : String.Empty;

    }
}