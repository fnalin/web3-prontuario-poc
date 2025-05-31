using MedLedger.Api.Blockchain.Endpoints;
using MedLedger.Api.Doctors;
using MedLedger.Api.Patients;
using MedLedger.Api.Records;

namespace MedLedger.Api.Endpoints;

public static class Maps
{
    public static void MapEndpoints(this WebApplication app)
    {
        //app.MapBlockchainsEndpoints();
        app.MapDoctorsEndpoints();
        app.MapPatientsEndpoints();
        app.MapRecordsEndpoints();
    }
}