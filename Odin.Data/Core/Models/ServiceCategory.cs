namespace Odin.Data.Core.Models
{
    public enum ServiceCategory
    {
        InitialConsultation = 1,
        WelcomePacket = 2,
        SettlingIn = 4,
        AreaOrientation = 8,
        SchoolFinding = 16,
        AccompaniedHomeFinding = 32,
        UnAccompaniedHomeFinding = 64,
        LeaseRenewal = 128,
        Departure = 256
    }
}