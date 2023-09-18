using BuscaBrasilApi.Interfaces;
using BuscaBrasilApi.Services.Api;

namespace BuscaBrasilApi.CSI.BuscaBrasilApi.Factories
{
    abstract class FGravaLog
    {
        public abstract IGravaLog Build(IConfiguracao config);
    }

    class FactoryGravaLog : FGravaLog
    {
        public override IGravaLog Build(IConfiguracao config)
        {
            return new GravaLog(config);
        }
    }
}
